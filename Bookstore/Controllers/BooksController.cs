using Microsoft.AspNetCore.Mvc;
using Bookstore.Services.Interfaces;
using Bookstore.Api.Models;
using Bookstore.Api.DTOs;

namespace Bookstore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int page = 1, int pageSize = 10)
        {
            var books = await _bookService.GetAllBooksAsync(page, pageSize);

            if (!books.Any())
                return NotFound();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpGet("unassigned")]
        public async Task<ActionResult<IEnumerable<Book>>> GetUnassignedBooks()
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
                return NotFound();

            return NoContent();
        }
    }
}
