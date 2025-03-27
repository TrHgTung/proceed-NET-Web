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
    public class ItemController : Controller
    {
        private readonly DataContext _context;

        public ItemController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        // [Route("api/Items")]
        public async Task<ActionResult<IEnumerable<ItemList>>> GetAllItems()
        {
            var getAllHangHoa = await _context.ItemLists.ToListAsync();
            if (getAllHangHoa == null){
                return NotFound();
            }
          
            return Ok(new { 
                getAllHangHoa = getAllHangHoa
            });
        }

        [HttpGet("{itemId}")]
        // [Route("api/Items")]
        public async Task<ActionResult<IEnumerable<ItemList>>> GetAllItems(string itemId)
        {
            var getAHangHoa = await _context.ItemLists.FindAsync(itemId);
            if (getAHangHoa == null){
                return NotFound();
            }
          
            return Ok(new { 
                getAHangHoa = getAHangHoa
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
