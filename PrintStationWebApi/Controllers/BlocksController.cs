﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintStationWebApi.Models.BL;
using PrintStationWebApi.Services.BL;
using PrintStationWebApi.Services.Cache;
using PrintStationWebApi.Services.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocksController : ControllerBase
    {
        private readonly IBookValidateService _bookValidateService;
        private readonly IBookSortingService _sortingService;
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService _cacheService;

        public BlocksController(IBookValidateService bookValidateService, IBookSortingService sortingService, IBookRepository bookRepository, ICacheService cacheService)
        {
            _bookValidateService = bookValidateService;
            _sortingService = sortingService;
            _bookRepository = bookRepository;
            _cacheService = cacheService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<List<Block[]>>> BlockHandler(params Block[] blocks)
        {
            if (blocks == null || blocks.Length < 1)
                return BadRequest();

            var barcodes = _bookValidateService.Parse(blocks.Select(c => c.Barcode)).ToList();
            var dbBooks = _cacheService.GetBooks(barcodes).ToList();
            if (dbBooks.Count < 1)
                dbBooks = await _bookRepository.GetBooksAsync(barcodes);

#pragma warning disable 4014
            _cacheService.AddRangeAsync(dbBooks); //Пусть задача улетает в параллельный поток, результат её здесь не важен.
#pragma warning restore 4014

            foreach (var block in blocks)
            {
                var barcode = _bookValidateService.Parse(block.Barcode);
                block.FullPath = dbBooks.FirstOrDefault(x => x.Barcode == barcode)?.CoverPath;
            }

            var sortedBooks = _sortingService.Sort(blocks) ?? throw new ApplicationException(nameof(blocks));

            return Ok(sortedBooks);
        }
    }
}
