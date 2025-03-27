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
            string divSubId = "0903705820";
            if (orderDto == null || orderDto.Amount == 0)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            int getLineNumber = _context.OrderDetails.Count() + 1;

            var orderDetail = new OrderDetail
            {
                LineNumber = getLineNumber,
                ItemID = (orderDto.ItemID),
                Quantity = orderDto.Quantity,
                Price = orderDto.Price,
                Amount = orderDto.Amount
            };

            var getCurrentDateTime = DateTime.UtcNow;
            // DateTime getCurrentDateTime = DateTime.UtcNow.Date;
            // var getOrderNo = ((_context.OrderMasters.Max(o => (int?)o.OrderNo) ?? 0) + 1).ToString();
            Random rnd = new Random();
            var randomNumber  = rnd.Next(11111, 99999);  
            var getOrderNo = "ORDERNO" + randomNumber.ToString();

            var oderMaster = new OrderMaster
            {
                OrderDate = getCurrentDateTime,
                OrderNo = getOrderNo,
                CustomerID = orderDto.CustomerID,
                TotalAmount = orderDto.TotalAmount,
                DivSubID = divSubId
            };

            _context.OrderMasters.Add(oderMaster);
            await _context.SaveChangesAsync();

            orderDetail.OrderMasterID = oderMaster.OrderMasterID;

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return Ok(new {
                orderDetail = orderDetail,
                oderMaster = oderMaster,
                msg = "Thành công"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromForm] UpdateDTO updtDto)
        {
            var checkOrderMaster = await _context.OrderMasters.FindAsync(id);
            if (id != order.OrderMasterID || checkOrderMaster == null){
                return BadRequest();
            }

            // var oderMaster = new OrderMaster
            // {
            //     OrderDate = getCurrentDateTime,
            //     OrderNo = getOrderNo,
            //     CustomerID = orderDto.CustomerID,
            //     TotalAmount = orderDto.TotalAmount,
            //     DivSubID = divSubId
            // };
            checkOrderMaster.OrderDate = orderMaster.OrderDate;
            checkOrderMaster.OrderNo = orderMaster.OrderNo;
            checkOrderMaster.CustomerID = orderMaster.CustomerID;
            checkOrderMaster.TotalAmount = orderMaster.TotalAmount;
            checkOrderMaster.DivSubID = orderMaster.DivSubID;

            _context.OrderMasters.Add(oderMaster);
            await _context.SaveChangesAsync();


            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}