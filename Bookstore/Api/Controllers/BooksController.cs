using Microsoft.AspNetCore.Mvc;
using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookFilter filter)
        {
            var books = await _bookService.GetAllBooksAsync(filter);

            if (!books.Any())
                return NotFound("There is no book in the bookstore");

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound("The book was not found");

            return Ok(book);
        }

        [HttpGet("unassigned")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetUnassignedBooks()
        {
            var books = await _bookService.GetUnassignedBooksAsync();

            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(CreateBookDto book)
        {
            await _bookService.CreateBookAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(UpdateBookDto book)
        {

            await _bookService.UpdateBookAsync(book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var isDeleted = await _bookService.DeleteBookAsync(id);

            if (!isDeleted)
                return NotFound("The book could not be deleted because it was not found");

            return NoContent();
        }
    }
}
