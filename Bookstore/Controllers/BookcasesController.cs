using Bookstore.Api.DTOs;
using Bookstore.Api.Models;
using Bookstore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookcasesController(IBookcasesService bookcasesService) : ControllerBase
    {
        private readonly IBookcasesService _bookcasesService = bookcasesService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookcaseDto>>> GetBookcases(int page = 0, int pageSize = 10)
        {

            var bookcases = await _bookcasesService.GetAllBookcasesAsync(page, pageSize);

            if (!bookcases.Any())
                return NotFound("We could not find any bookcase registered");

            return Ok(bookcases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookcaseDto>> GetBookcase(int id)
        {
            var bookcase = await _bookcasesService.GetBookcaseByIdAsync(id);

            if (bookcase == null)
                return NotFound("We could not find the bookcase");

            return bookcase;
        }

        [HttpPost]
        public async Task<ActionResult<Bookcase>> CreateBookcase(CreateBookcaseDto bookcase)
        {
            await _bookcasesService.CreateBookcaseAsync(bookcase);

            return CreatedAtAction(nameof(GetBookcase), new { id = bookcase.Id }, bookcase);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookcase(UpdateBookcaseDto bookcase)
        {

            await _bookcasesService.UpdateBookcaseAsync(bookcase);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookcase(int id)
        {
            var isDeleted = await _bookcasesService.DeleteBookcaseAsync(id);

            if (!isDeleted)
                return NotFound("We could not find the bookcase");

            return NoContent();
        }
    }
}
