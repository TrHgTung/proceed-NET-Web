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
    public class EditOrderController : Controller
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditOrderController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // B1:  GET: hien thi toan bo OrderMaster
        [HttpGet]
        [Route("/selectOM")]
        public async Task<ActionResult<IEnumerable<OrderMaster>>> GetOM()
        {
            var getOdersMaster = await _context.OrderMasters.ToListAsync();
            if(getOdersMaster == null){
                return NotFound(new {
                    message = "Không có dữ liệu"
                });
            }
            return Ok(new {
                getOdersMaster = getOdersMaster
            });
        }

        // B2:  GET: hien thi toan bo OrderDetail dua tren OrderMaster (OrderMasterId)
        [HttpGet]
        [Route("/selectOM/{omid}")]
        public async Task<ActionResult<IEnumerable<OrderMaster>>> GetOM(Guid omid)
        {
            // var getAllOrderDetails = await _context.OrderDetails.ToListAsync();
            var currentOrderMasterID = _httpContextAccessor.HttpContext.Session;
            currentOrderMasterID.SetString("CurrentOrderMasterID", omid.ToString());

            var getAllOrderDetails  = await _context.OrderDetails
                        .Join(_context.OrderMasters,
                            od => od.OrderMasterID,
                            om => om.OrderMasterID,
                            (od, om) => new 
                            { 
                                od.RowDetailID,
                                od.LineNumber,
                                od.ItemID,
                                od.Quantity,
                                od.Price,
                                od.Amount,
                                od.OrderMasterID
                            }) 
                        .Where(a => a.OrderMasterID.ToString() == currentOrderMasterID.ToString())
                        .ToListAsync();

            if(getAllOrderDetails == null || !getAllOrderDetails.Any()){
                return NotFound(new {
                    message = "Không có dữ liệu"
                });
            }
            return Ok(new {
                getAllOrderDetails = getAllOrderDetails
            });
        }

        // B3:  PUT: sua: Ma hang hoa (ItemID), So luong (Quantity), Don gia (Price), Thanh tien (Amount)
        //      - ItemID  ,  Quantity  ,  Price  ,  Amount=Quantity*Price

        // B4:  PUT: update lai OrderMaster dua tren OrderMasterId cua cac OrderDetail (tuong tu route Submit ben OrderHandle controller)
        







        // // form sua update se lay api nayf
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