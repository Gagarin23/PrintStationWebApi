using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrintStationWebApi.Models.BL;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services;
using PrintStationWebApi.Services.BL;
using PrintStationWebApi.Services.Cache;
using PrintStationWebApi.Services.DataBase;

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

        [HttpPost]
        public async Task<ActionResult<List<Cover[]>>> CoverHandler(params Cover[] covers)
        {
            if (covers == null) 
                return BadRequest();

            var barcodes = _validateService.Parse(covers.Select(c => c.Barcode).ToArray()).ToArray();
            var dbBooks = _cacheService.GetBooks(barcodes).ToArray();
            if (dbBooks.Length < 1)
                dbBooks = await _bookRepository.GetBooksAsync(barcodes);

#pragma warning disable 4014
            _cacheService.AddRangeAsync(dbBooks); //Пусть задача улетает в параллельный поток, результат её здесь не важен.
#pragma warning restore 4014

            //covers = covers.Zip(dbBooks, (cover, book) => new Cover(cover, book));
            //Можно было бы сделать конструктор для обложки под эти параметры.
            //Выглядело бы красиво, но плодить копии коллекций не хочу. 
            //Поэтому старый добрый for:

            for (int i = 0; i < covers.Length; i++)
            {
                covers[i].FullPath = dbBooks[i].CoverPath;
            }

            var sortedBooks = _sortingService.Sort(covers) ?? throw new ApplicationException(nameof(covers));
            return Ok(sortedBooks);
        }
    }
}
