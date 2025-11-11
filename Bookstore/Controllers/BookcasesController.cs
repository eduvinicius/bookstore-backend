using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Api.Data;
using Bookstore.Api.Models;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookcasesController: ControllerBase
    {
        private readonly BookstoreContext _context;
        public BookcasesController(BookstoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookcase>>> GetBookcases()
        {
            return await _context.Bookcases
                .Include(b => b.Books)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bookcase>> GetBookcase(int id)
        {
            var bookcase = await _context.Bookcases
                .Include(b => b.Books)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bookcase == null)
                return NotFound();

            return bookcase;
        }

        [HttpPost]
        public async Task<ActionResult<Bookcase>> CreateBookcase(Bookcase bookcase)
        {
            _context.Bookcases.Add(bookcase);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookcase), new { id = bookcase.Id }, bookcase);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookcase(int id, Bookcase bookcase)
        {
            if (id != bookcase.Id)
                return BadRequest();

            _context.Entry(bookcase).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookcase(int id)
        {
            var bookcase = await _context.Bookcases.FindAsync(id);
            if (bookcase == null)
                return NotFound();

            _context.Bookcases.Remove(bookcase);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
