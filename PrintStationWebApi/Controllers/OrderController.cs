using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrintStationWebApi.Models.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public async void Get(params Cover[] covers)
        {
            foreach (var cover in covers)
            {
                Console.WriteLine(cover);
            }
        }

        [HttpGet]
        public IEnumerable<string> Get(params Block[] blocks)
        {
            return new string[] { "value1", "value2" };
        }
    }
}
