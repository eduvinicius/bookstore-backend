using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.App.Services;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(
        IBookService bookService, 
        IBookImportService bookImportSeervice
        ) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;
        private readonly IBookImportService _bookImportService = bookImportSeervice;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookFilter filter)
        {
            var books = await _bookService.GetAllBooksAsync(filter);

            if (!books.Any())
                return NotFound("There is no book in the bookstore");

            return Ok(books);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound("The book was not found");

            return Ok(book);
        }

        [Authorize]
        [HttpGet("unassigned")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetUnassignedBooks()
        {
            var books = await _bookService.GetUnassignedBooksAsync();

            return Ok(books);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(CreateBookDto book)
        {
            await _bookService.CreateBookAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [Authorize]
        [HttpPost("import/google/{googleBookId}")]
        public async Task<ActionResult<BookDto>> ImportFromGoogle(string googleBookId)
        {
            var book = await _bookImportService.ImportFromGoogleAsync(googleBookId);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBook(UpdateBookDto book)
        {

            await _bookService.UpdateBookAsync(book);

            return NoContent();
        }

        [Authorize]
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
