
using Extensions;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserOnly")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User.GetUserId();

            try
            {
                var order = await _orderRepository.PlaceOrderAsync(userId);
                return Ok(new { isSuccess = true, message = "Order placed successfully", data = order });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex, data = (object)null });
            }
        }

        [HttpGet("viewOrders")]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = User.GetUserId();

            try
            {
                var orders = await _orderRepository.GetOrdersAsync(userId);
                return Ok(new { isSuccess = true, message = "Data retrieved successfully", data = orders });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }
    }
}