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
    // [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("api/Customers")]
        public async Task<ActionResult<IEnumerable<CustomerList>>> GetAllCustomerInfo()
        {
            var getAllCustomers = await _context.CustomerLists.ToListAsync();
          
            return Ok(new { 
                getAllCustomers = getAllCustomers
            });
        }

        // [HttpGet]
        // [Route("answer/edit/{questionId}")]
        // [Authorize(Policy = "AdminPolicy")]
        // public async Task<ActionResult<IEnumerable<Questions>>> GetQuestion(int questionId)
        // {
        //     var userId = HttpContext.Session.GetString("LecturerEmail");

        //     if (string.IsNullOrEmpty(userId))
        //     {
        //         return Unauthorized("Yêu cầu xác thực giảng viên");
        //     }
          
        //     var getAnswersByQuestionId = await _context.Answer
        //                 .Where(a => a.QuestionId == questionId)
        //                 .Select(a => new {
        //                         a.Id,
        //                         a.AnswerContent,
        //                         a.AnswerImage
        //                     })
        //                 .ToListAsync();

        //     return Ok(new { 
        //         userId = userId,
        //         getFourAnswersByQuestionId = getAnswersByQuestionId,
        //     });
        // }

        
    }
}
