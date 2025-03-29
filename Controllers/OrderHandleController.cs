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
using Microsoft.AspNetCore.Http;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHandleController : Controller
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderHandleController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
        [Route("init")]
        public async Task<ActionResult> InitOrderMaster(IHttpContextAccessor httpContextAccessor)
        {
            var initGuid = Guid.NewGuid();
            var initOrderMaster = new OrderMaster
            {
                OrderMasterID = initGuid,
                OrderDate = DateTime.UtcNow,
                OrderNo = "0",
                CustomerID = "0",
                TotalAmount = 0,
                DivSubID = "0903705820"
            };

            var currentOrderMasterID = _httpContextAccessor.HttpContext.Session;
            currentOrderMasterID.SetString("CurrentOrderMasterID", initGuid.ToString());

            _context.OrderMasters.Add(initOrderMaster);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                orderMaster = initOrderMaster,
                message = "Đã khởi tạo OrderMaster"
            });
        }

        [HttpPost]
         [Route("add")]
        public async Task<ActionResult> CreateSingleOrderLine([FromForm] CreateDTO.OrderDetail orderDto, IHttpContextAccessor httpContextAccessor)
        {
            var currentOrderMasterID = HttpContext.Session.GetString("CurrentOrderMasterID");
            if (currentOrderMasterID == null)
            {
                return BadRequest("Có lỗi hệ thống");
            }
            int getLineNumber = _context.OrderDetails.Count() + 1;

            var orderDetail = new OrderDetail
            {
                OrderMasterID = Guid.Parse(currentOrderMasterID),
                LineNumber = getLineNumber,
                ItemID = (orderDto.ItemID), // lay tu giao dien UI
                Quantity = orderDto.Quantity, // lay tu giao dien
                Price = orderDto.Price, // lay tu giao dien
                Amount = (decimal)(orderDto.Quantity * orderDto.Price) // tu dong
            };

            // var getCurrentDateTime = DateTime.UtcNow.Date;
            // Random rnd = new Random();
            // var randomNumber  = rnd.Next(11111, 99999);  
            // var getOrderNo = "ORDERNO" + randomNumber.ToString();

            // var oderMaster = new OrderMaster
            // {
            //     OrderDate = getCurrentDateTime,
            //     OrderNo = getOrderNo,
            //     CustomerID = orderDto.CustomerID,
            //     TotalAmount = orderDto.TotalAmount,
            //     DivSubID = divSubId
            // };

            // _context.OrderMasters.Add(oderMaster);
            // await _context.SaveChangesAsync();

            // orderDetail.OrderMasterID = oderMaster.OrderMasterID;

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return Ok(new {
                orderDetail = orderDetail,
                // oderMaster = oderMaster,
                msg = "Đã thêm sản phẩm vào hàng chờ cua hóa đơn"
            });
        }

        [HttpPut]
         [Route("submit")]
        public async Task<ActionResult> CreateOrderSubmit([FromForm] CreateDTO.OrderMaster orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            string divSubId = "0903705820";
            var currentOrderMasterID = HttpContext.Session.GetString("CurrentOrderMasterID"); // OrderMasterID
            var getCurrentDateTime = DateTime.UtcNow;
            // var getOrderDetailData = await _context.OrderDetails
            //             .Join(_context.OrderMasters,
            //                 od => od.OrderMasterID,
            //                 om => om.OrderMasterID,
            //                 (od, om) => new 
            //                 { 
            //                     Quantity = od.Quantity,
            //                     Amount = od.Amount,
            //                     OrderMasterID = om.OrderMasterID
            //                 }) 
            //             .Where(a => a.OrderMasterID.ToString() == currentOrderMasterID)
            //             // .Sum(abc => abc.Quantity)
            //             .ToListAsync();

            // sum Quantity (OrderNo)
            var totalQuantity  = _context.OrderDetails
                        .Join(_context.OrderMasters,
                            od => od.OrderMasterID,
                            om => om.OrderMasterID,
                            (od, om) => new 
                            { 
                                Quantity = od.Quantity,
                                Amount = od.Amount,
                                OrderMasterID = om.OrderMasterID
                            }) 
                        .Where(a => a.OrderMasterID.ToString() == currentOrderMasterID)
                        .Sum(b => b.Quantity)
                        .ToString();

            // sum cua Amount (TotalAmount)
            var totalAmount = _context.OrderDetails
                        .Join(_context.OrderMasters,
                            od => od.OrderMasterID,
                            om => om.OrderMasterID,
                            (od, om) => new 
                            { 
                                Quantity = od.Quantity,
                                Amount = od.Amount,
                                OrderMasterID = om.OrderMasterID
                            }) 
                        .Where(a => a.OrderMasterID.ToString() == currentOrderMasterID)
                        .Sum(b => b.Amount);

            var getOrderMaster = await _context.OrderMasters.FirstOrDefaultAsync(o => o.OrderMasterID.ToString() == currentOrderMasterID);

            if (getOrderMaster == null)
            {
                return NotFound(new {
                    message = "Không tìm được dữ liệu"
                });
            }

            // var createOrderMaster = new OrderMaster
            // {
            //     OrderDate = getCurrentDateTime,
            //     OrderNo = totalQuantity,
            //     CustomerID = orderDto.CustomerID,
            //     TotalAmount = totalAmount,
            //     DivSubID = divSubId
            // };
            
            // _context.OrderMasters.Add(createOrderMaster);
            getOrderMaster.OrderDate = getCurrentDateTime;
            getOrderMaster.OrderNo = totalQuantity;
            getOrderMaster.CustomerID = orderDto.CustomerID;
            getOrderMaster.TotalAmount = totalAmount;
            getOrderMaster.DivSubID = divSubId;

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("CurrentOrderMasterID");

            return Ok(new {
                // test = getOrderDetailData,
                // duLieuDaThem = createOrderMaster,
                totalQuantity = totalQuantity,
                totalAmount = totalAmount,
                msg = "Thành công"
            });
        }

        // form sua update se lay api nayf
        // [HttpPut("update/{orderMasterId}")]
        // public async Task<IActionResult> UpdateOrder(Guid orderMasterId, [FromForm] UpdateOrderDto updtDto)
        // {
        //     var checkOrderMaster = await _context.OrderMasters.FindAsync(orderMasterId);
        //     var getCurrentDateTime = DateTime.UtcNow.Date;

        //     if (orderMasterId != checkOrderMaster.OrderMasterID || checkOrderMaster == null){
        //         return NotFound(new {
        //             message = "Không tìm được OrderMaster"
        //         });
        //     }

        //     // cap nhat oderMaster
        //     checkOrderMaster.OrderNo = updtDto.OrderNo;
        //     checkOrderMaster.TotalAmount = updtDto.TotalAmount;

        //     var getOrderDetail = await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderMasterID == orderMasterId);
           
        //     if (getOrderDetail == null)
        //     {
        //         return NotFound(new {
        //             message = "Không tìm được OrderDetail"
        //         });
        //     }
        //     // cap nhat OrderDetails
        //     getOrderDetail.ItemID = updtDto.ItemID;
        //     getOrderDetail.Quantity = updtDto.Quantity;
        //     getOrderDetail.Price = updtDto.Price;
        //     // getOrderDetail.Amount = updtDto.Amount;
        //     getOrderDetail.Amount = (decimal)(getOrderDetail.Price * getOrderDetail.Quantity);

        //     await _context.SaveChangesAsync();

        //     return Ok(new
        //     {
        //         message = "Thành công cập nhật hóa đơn",
        //         orderMaster = checkOrderMaster,
        //         OrderDetail = getOrderDetail
        //     });
        // }
    }
}