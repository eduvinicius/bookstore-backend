using Bookstore.App.Filters;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Data;
using Bookstore.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Repositories
{
    public class BookcaseRepository(BookstoreContext context) : Repository<Bookcase>(context), IBookcaseRepository
    {
        protected override IQueryable<Bookcase> WithIncludes()
        {
            return _dbSet.Include(bc => bc.Books);
        }

        public async Task<IEnumerable<Bookcase>> GetAllAsync(BookcaseFilter filter)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(bc => bc.Books)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(bc => bc.Name.Contains(filter.Name));

            return await query
                .Where(bc => filter.UserId == bc.UserId)
                .OrderBy(bc => bc.Id)
                .Skip(filter.Page)
                .Take(filter.PageSize)
                .ToListAsync();
        }

    }
}
