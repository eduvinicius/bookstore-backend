using Bookstore.Api.Data;
using Bookstore.Api.Models;
using Bookstore.Api.DTOs;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Bookstore.App.Filters;

namespace Bookstore.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BookstoreContext context) : base(context) { }

        protected override IQueryable<Book> WithIncludes()
        {
            return _dbSet.Include(b => b.Bookcase);
        }

        public async Task<IEnumerable<Book>> GetByBookcaseIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(b => b.BookcaseId == id)
                .Include(b => b.Bookcase)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync(BookFilter filter)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(b => b.Bookcase)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(b => b.Title.Contains(filter.Title));

            if (!string.IsNullOrWhiteSpace(filter.Author))
                query = query.Where(b => b.Author.Contains(filter.Author));

            if (!string.IsNullOrWhiteSpace(filter.Genre))
                query = query.Where(b => b.Genre.Contains(filter.Genre));

            if (filter.IsRead.HasValue)
                query = query.Where(b => b.IsRead == filter.IsRead.Value);

            return await query
                .OrderBy(b => b.CreatedDate)
                .Skip(filter.Page)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetUnassignedBooksAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(b => b.BookcaseId == null)
                .Include(b => b.Bookcase)
                .ToListAsync();
        }

        public async Task<List<Book>> GetByIdsListAsync(List<int> ids)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(b => ids.Contains(b.Id))
                .Include(b => b.Bookcase)
                .ToListAsync();
        }

    }
}
