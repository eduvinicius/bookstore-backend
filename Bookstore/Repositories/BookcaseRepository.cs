using Bookstore.Api.Data;
using Bookstore.Api.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class BookcaseRepository: IBookcaseRepository
    {
        private readonly BookstoreContext _context;

        public BookcaseRepository(BookstoreContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Bookcase bookcase)
        {
            await _context.Bookcases.AddAsync(bookcase);
        }

        public async Task<Bookcase?> GetByIdAsync(int id)
        {
            return await _context.Bookcases
                .Include(bc => bc.Books)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bookcase>> GetAllAsync(int skip, int take)
        {
            return await _context.Bookcases
                .OrderBy(bc => bc.Id)
                .Skip(skip)
                .Take(take)
                .Include(bc => bc.Books)
                .ToListAsync();
        }

        public void Update(Bookcase bookcase)
        {
            _context.Bookcases.Update(bookcase);
        }

        public void Delete(Bookcase bookcase)
        {
            _context.Bookcases.Remove(bookcase);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
