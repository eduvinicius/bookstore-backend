using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookcasesController(
        IBookcasesService bookcasesService,
        ICurrentUserService currentUserService
        ) : ControllerBase
    {
        private readonly IBookcasesService _bookcasesService = bookcasesService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookcaseDto>>> GetBookcases([FromQuery] BookcaseFilter filter)
        {

            filter.UserId = this._currentUserService.UserId;

            var bookcases = await _bookcasesService.GetAllBookcasesAsync(filter);

            if (!bookcases.Any())
                return NotFound("We could not find any bookcase registered");

            return Ok(bookcases);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookcaseDto>> GetBookcase(int id)
        {
            var bookcase = await _bookcasesService.GetBookcaseByIdAsync(id);
            return Ok(bookcase);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Bookcase>> CreateBookcase(CreateBookcaseDto bookcase)
        {

            var userId = this._currentUserService.UserId;

            await _bookcasesService.CreateBookcaseAsync(bookcase, userId);

            return CreatedAtAction(nameof(GetBookcase), new { id = bookcase.Id }, bookcase);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBookcase(UpdateBookcaseDto bookcase)
        {

            await _bookcasesService.UpdateBookcaseAsync(bookcase);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookcase(int id)
        {
            await _bookcasesService.DeleteBookcaseAsync(id);
            return NoContent();
        }
    }
}
