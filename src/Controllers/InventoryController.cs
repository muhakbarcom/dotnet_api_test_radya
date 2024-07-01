using Dtos.Inventory;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet("index")]

        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _inventoryRepository.GetAllBooksAsync();
                return Ok(new { isSuccess = true, message = "Inventory retrieved successfully", data = books });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }

        [HttpPost("addStock/{id}")]

        public async Task<IActionResult> AddStock(int id, [FromBody] AddStockDto addStockDto)
        {
            if (addStockDto.Quantity < 1)
            {
                return BadRequest(new { isSuccess = false, message = "Quantity must be at least 1" });
            }

            try
            {
                var book = await _inventoryRepository.AddStockAsync(id, addStockDto.Quantity);
                return Ok(new { isSuccess = true, message = "Stock added successfully", data = book });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }

        [HttpPost("reduceStock/{id}")]

        public async Task<IActionResult> ReduceStock(int id, [FromBody] ReduceStockDto reduceStockDto)
        {
            if (reduceStockDto.Quantity < 1)
            {
                return BadRequest(new { isSuccess = false, message = "Quantity must be at least 1" });
            }

            try
            {
                var book = await _inventoryRepository.ReduceStockAsync(id, reduceStockDto.Quantity);
                return Ok(new { isSuccess = true, message = "Stock reduced successfully", data = book });
            }
            catch (System.Exception ex)
            {
                if (ex.Message == "Not enough stock available")
                {
                    return BadRequest(new { isSuccess = false, message = ex.Message, data = (object)null });
                }

                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }

        [HttpDelete("deleteBook/{id}")]

        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var success = await _inventoryRepository.DeleteBookAsync(id);
                if (!success)
                {
                    return NotFound(new { isSuccess = false, message = "Book not found" });
                }

                return Ok(new { isSuccess = true, message = "Book deleted successfully", data = (object)null });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { isSuccess = false, message = ex.Message, data = (object)null });
            }
        }
    }
}