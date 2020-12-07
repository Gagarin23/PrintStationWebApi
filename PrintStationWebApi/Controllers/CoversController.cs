using Microsoft.AspNetCore.Authorization;
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
    public class CoversController : ControllerBase
    {
        private readonly IValidateService _validateService;
        private readonly IBookSortingService _sortingService;
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService _cacheService;

        public CoversController(IValidateService validateService, IBookSortingService sortingService, IBookRepository bookRepository, ICacheService cacheService)
        {
            _validateService = validateService;
            _sortingService = sortingService;
            _bookRepository = bookRepository;
            _cacheService = cacheService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<List<Cover[]>>> CoverHandler(params Cover[] covers)
        {
            if (covers == null || covers.Length < 1)
                return BadRequest();

            var barcodes = _validateService.Parse(covers.Select(c => c.Barcode)).ToList();
            var dbBooks = _cacheService.GetBooks(barcodes).ToList();
            if (dbBooks.Count < 1)
                dbBooks = await _bookRepository.GetBooksAsync(barcodes);

#pragma warning disable 4014
            _cacheService.AddRangeAsync(dbBooks); //Пусть задача улетает в параллельный поток, результат её здесь не важен.
#pragma warning restore 4014

            foreach (var cover in covers)
            {
                var barcode = _validateService.Parse(cover.Barcode);
                cover.FullPath = dbBooks.FirstOrDefault(x => x.Barcode == barcode)?.CoverPath;
            }

            var sortedBooks = _sortingService.Sort(covers) ?? throw new ApplicationException(nameof(covers));

            return Ok(sortedBooks);
        }
    }
}
