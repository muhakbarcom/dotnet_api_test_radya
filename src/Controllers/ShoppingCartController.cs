using Dtos.ShoppingCart;
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
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly UserManager<User> _userManager;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, UserManager<User> userManager)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userManager = userManager;
        }

        [HttpPost("addToCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var userId = User.GetUserId();

            try
            {
                var cartItem = await _shoppingCartRepository.AddToCartAsync(userId, addToCartDto.BookId, addToCartDto.Quantity);
                return Ok(new { isSuccess = true, message = "Book added to cart successfully", data = cartItem });
            }
            catch (System.Exception ex)
            {
                return StatusCode(422, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }

        [HttpPut("updateCart/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] UpdateCartDto updateCartDto)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.UpdateCartAsync(id, updateCartDto.Quantity);
                return Ok(new { isSuccess = true, message = "Cart updated successfully", data = cartItem });
            }
            catch (System.Exception ex)
            {
                return StatusCode(422, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }

        [HttpDelete("removeFromCart/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            try
            {
                var success = await _shoppingCartRepository.RemoveFromCartAsync(id);
                if (!success)
                {
                    return NotFound(new { isSuccess = false, message = "Cart item not found" });
                }

                return Ok(new { isSuccess = true, message = "Book removed from cart successfully", data = (object)null });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }

        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> ViewCart()
        {
            try
            {
                var userId = User.GetUserId();
                var cartItems = await _shoppingCartRepository.GetCartItemsAsync(userId);
                return Ok(new { isSuccess = true, message = "Cart retrieved successfully" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }
    }
}