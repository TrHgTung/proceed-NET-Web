using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json.Serialization;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : Controller
    {
        private readonly DataContext _context;

        public InfoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerList>>> GetTheNumberOfOrderMaster()
        {
            var getNumberOfOrder = await _context.OrderMasters.CountAsync();
            if (getNumberOfOrder == 0){
                return NotFound();
            }
          
            return Ok(new { 
                getNumberOfOrder = getNumberOfOrder
            });
        }
    }
}
