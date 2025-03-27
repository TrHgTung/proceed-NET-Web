using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Models;
using webapp.Dto;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json.Serialization;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHandleController : Controller
    {
        private readonly DataContext _context;

        public OrderHandleController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderMaster>>> GetOrders()
        {
            var getOdersDetails = await _context.OrderMasters.ToListAsync();
            if(getOdersDetails == null){
                return NotFound(new {
                    message = "Không có dữ liệu"
                });
            }
            return Ok(new {
                getOdersDetails = getOdersDetails
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromForm] OrderDto orderDto)
        {
            if (orderDto == null || orderDto.Amount == 0)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            int getLineNumber = 0;
            getLineNumber = _context.OrderDetails.Count() + 1;

            var orderDetail = new OrderDetail
            {
                LineNumber = getLineNumber,
                ItemID = (orderDto.ItemID),
                Quantity = orderDto.Quantity,
                Price = orderDto.Price,
                Amount = orderDto.Amount
            };

            // string getCurrentDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string getCurrentDateTime = DateTime.UtcNow;
            // DateTime getCurrentDateTime = DateTime.UtcNow.Date;
            // getCurrentDateTime.ToString("dd/MM/yyyy");
            var getOrderNo = ((_context.OrderMasters.Max(o => (int?)o.OrderNo) ?? 0) + 1).ToString();
            decimal totalAmt = orderDto.TotalAmount;
            var oderMaster = new OrderMaster
            {
                OrderDate = getCurrentDateTime,
                OrderNo = getOrderNo,
                CustomerID = orderDto.CustomerID,
                TotalAmount = totalAmt,
                DivSubID = "0903705820"
            };

            _context.OrderMasters.Add(oderMaster);
            _context.OrderDetails.Add(orderDetail);
           
            await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetOrders),
            //     new {
            //         id = order.OrderMasterID
            //     }, order);
            return Ok(new {
                orderDetail = orderDetail,
                oderMaster = oderMaster,
                msg = "Thành công"
            });
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateOrder(int id, OrderMaster order)
        // {
        //     if (id != order.OrderMasterID)
        //         return BadRequest();

        //     _context.Entry(order).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();
        //     return NoContent();
        // }
    }
}