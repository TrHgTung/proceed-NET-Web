using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Models;
// using webapp.Dto;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json.Serialization;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        // [Route("api/Customers")]
        public async Task<ActionResult<IEnumerable<CustomerList>>> GetAllCustomerInfo()
        {
            var getAllCustomers = await _context.CustomerLists.ToListAsync();
            if (getAllCustomers == null){
                return NotFound();
            }
          
            return Ok(new { 
                getAllCustomers = getAllCustomers
            });
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerList>> GetACustomer(string customerId)
        {
            var getACustomer = await _context.CustomerLists.FindAsync(customerId);
            if (getACustomer == null)
                return NotFound();
                
            return Ok(new {
                getACustomer = getACustomer
            });
        }
    }
}
