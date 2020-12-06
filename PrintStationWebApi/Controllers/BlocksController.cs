using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrintStationWebApi.Models.BL;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocksController : ControllerBase
    {
        [HttpPost]
        public void BlockHandler(params Block[] blocks)
        {
            foreach (var block in blocks)
            {
                Console.WriteLine(block);
            }
        }
    }
}
