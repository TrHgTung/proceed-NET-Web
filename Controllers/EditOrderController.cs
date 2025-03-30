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
        public async Task<ActionResult<IEnumerable<OrderMaster>>> GetDetails_OM(Guid omid)
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
                        .Where(a => a.OrderMasterID.ToString() == omid.ToString())
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
        [HttpPut]
        [Route("updateOM/{rdid}")] // rdid la rowDetailID
        public async Task<IActionResult> SaveOrderDetails(Guid rdid, [FromForm] UpdateOrderDto updtDto)
        {
            var checkOrderDetail = await _context.OrderDetails.FindAsync(rdid);

            if (rdid != checkOrderDetail.RowDetailID || checkOrderDetail == null){
                return NotFound(new {
                    message = "Dư liệu không hợp lệ"
                });
            }

            // var getAllOrderDetails  = await _context.OrderDetails
            //             .Where(od => od.OrderMasterID.ToString() == omid.ToString())
            //             .ToListAsync();

            // if(getAllOrderDetails == null || !getAllOrderDetails.Any()){
            //     return NotFound(new {
            //         message = "Không có dữ liệu"
            //     });
            // }

            checkOrderDetail.ItemID = updtDto.ItemID;
            checkOrderDetail.Quantity = updtDto.Quantity;
            checkOrderDetail.Price = updtDto.Price;
            checkOrderDetail.Amount = (decimal)(updtDto.Quantity * updtDto.Price);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đã sửa lại dữ liệu"
            });
        }

        // B4:  PUT: update lai OrderMaster dua tren OrderMasterId cua cac OrderDetail (tuong tu route Submit ben OrderHandle controller)
        [HttpPut]
        [Route("updateOM/save")] // omid la OrderMasterId
        public async Task<IActionResult> SaveOrderMaster( [FromForm] UpdateOMDto updtDto)
        {
            // var getOrderMaster = await _context.OrderMasters.FindAsync(omid);
            var currentOrderMasterID = HttpContext.Session.GetString("CurrentOrderMasterID");
            var getOrderMaster = await _context.OrderMasters.FirstOrDefaultAsync(o => o.OrderMasterID.ToString() == currentOrderMasterID);

            if (currentOrderMasterID != getOrderMaster.OrderMasterID.ToString() || getOrderMaster == null){
                return NotFound(new {
                    message = "Dư liệu không hợp lệ"
                });
            }

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

            getOrderMaster.CustomerID = updtDto.CustomerID;
            getOrderMaster.OrderNo = totalQuantity;
            getOrderMaster.TotalAmount = totalAmount;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đã cập nhật toàn bộ thông tin của hóa đơn"
            });
        }
   
    }
}