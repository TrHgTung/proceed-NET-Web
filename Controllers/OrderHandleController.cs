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

            var getCurrentDateTime = DateTime.UtcNow.Date;
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

        // form sua update se lay api nayf
        [HttpPut("update/{orderMasterId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderMasterId, [FromForm] UpdateOrderDto updtDto)
        {
            var checkOrderMaster = await _context.OrderMasters.FindAsync(orderMasterId);
            var getCurrentDateTime = DateTime.UtcNow.Date;

            if (orderMasterId != checkOrderMaster.OrderMasterID || checkOrderMaster == null){
                return NotFound(new {
                    message = "Không tìm được OrderMaster"
                });
            }

            // cap nhat oderMaster
            checkOrderMaster.OrderNo = updtDto.OrderNo;
            checkOrderMaster.TotalAmount = updtDto.TotalAmount;

            var getOrderDetail = await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderMasterID == orderMasterId);
           
            if (getOrderDetail == null)
            {
                return NotFound(new {
                    message = "Không tìm được OrderDetail"
                });
            }
            // cap nhat OrderDetails
            getOrderDetail.ItemID = updtDto.ItemID;
            getOrderDetail.Quantity = updtDto.Quantity;
            getOrderDetail.Price = updtDto.Price;
            // getOrderDetail.Amount = updtDto.Amount;
            getOrderDetail.Amount = (decimal)(getOrderDetail.Price * getOrderDetail.Quantity);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thành công cập nhật hóa đơn",
                orderMaster = checkOrderMaster,
                OrderDetail = getOrderDetail
            });
        }
    }
}