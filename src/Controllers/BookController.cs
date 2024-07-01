
using Dtos.Book;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBooks([FromQuery] string genre, [FromQuery] string author)
        {
            var books = await _bookRepository.GetAllBooksAsync(genre, author);
            return Ok(new { isSuccess = true, message = "Books retrieved successfully", data = books });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity
            };

            var createdBook = await _bookRepository.CreateBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, new { isSuccess = true, message = "Book created successfully", data = createdBook });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { isSuccess = false, message = "Book not found" });
            }

            return Ok(new { isSuccess = true, message = "Book retrieved successfully", data = book });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            var book = new Book
            {
                Id = id,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity
            };

            try
            {
                var updatedBook = await _bookRepository.UpdateBookAsync(book);
                return Ok(new { isSuccess = true, message = "Book updated successfully", data = updatedBook });
            }
            catch
            {
                return StatusCode(500, new { isSuccess = false, message = "An error occurred while updating the book" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookRepository.DeleteBookAsync(id);
            if (!success)
            {
                return NotFound(new { isSuccess = false, message = "Book not found" });
            }

            return Ok(new { isSuccess = true, message = "Book deleted successfully" });
        }
    }
}