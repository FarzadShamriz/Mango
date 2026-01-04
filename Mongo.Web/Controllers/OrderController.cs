using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> OrderIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> orderHeaders;
            string userId = string.Empty;

            if (User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDto response = _orderService.GetAllOrders(userId).GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                orderHeaders = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
                switch (status.ToLower())
                {
                    case "approved":
                        orderHeaders = orderHeaders.Where(o => o.Status == SD.Status_Approved).ToList();
                        break;
                    case "readyforpickup":
                        orderHeaders = orderHeaders.Where(o => o.Status == SD.Status_ReadyForPickup).ToList();
                        break;
                    case "cancelled":
                        orderHeaders = orderHeaders.Where(o => o.Status == SD.Status_Cancelled).ToList();
                        break;
                }
            }
            else
            {
                orderHeaders = new List<OrderHeaderDto>();
            }
            return Json(new { data = orderHeaders });
        }

        [Authorize]
        public IActionResult OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            var response = _orderService.GetOrder(orderId).GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            }
            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDto);
        }

        [HttpPost]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully!";
            }
            else
            {
                TempData["error"] = "Error!";
            }
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });

        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully!";
            }
            else
            {
                TempData["error"] = "Error!";
            }
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });

        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully!";
            }
            else
            {
                TempData["error"] = "Error!";
            }
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });

        }
    }
}
