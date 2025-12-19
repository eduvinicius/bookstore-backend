using Bookstore.Api.DTOs.External;
using Bookstore.App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers
{
    [ApiController]
    [Route("api/external-books")]
    public class ExternalBooksController(IGoogleBooksService googleBooksService) : ControllerBase
    {
        private readonly IGoogleBooksService _googleBooksService = googleBooksService;

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GoogleBookDto>>> Search(
            [FromQuery] string query,
            [FromQuery] int page = 0,
            [FromQuery] int maxResults = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var books = await _googleBooksService.SearchAsync(query, maxResults, page);

            return Ok(books);
        }
    }

}
