using Bookstore.Api.Data;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {
        protected readonly BookstoreContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(BookstoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        protected virtual IQueryable<T> WithIncludes()
        {
            return _dbSet;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await WithIncludes()
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
