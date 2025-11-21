using Bookstore.Api.Data;
using Bookstore.Api.Models;
using Bookstore.App.Filters;
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

        public async Task<IEnumerable<Bookcase>> GetAllAsync(BookcaseFilter filter)
        {
            var query = _context.Bookcases
                .AsNoTracking()
                .Include(bc => bc.Books)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(bc => bc.Name.Contains(filter.Name));

            return await query
                .OrderBy(bc => bc.Id)
                .Skip(filter.Page)
                .Take(filter.PageSize)
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
