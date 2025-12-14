using Bookstore.App.Filters;
using Bookstore.Domain.Entities;

namespace Bookstore.Infrastructure.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> GetByIdsListAsync(List<int> ids);
        Task<IEnumerable<Book>> GetAllAsync(BookFilter filter);
        Task<IEnumerable<Book>> GetByBookcaseIdAsync(int id);
        Task<IEnumerable<Book>> GetUnassignedBooksAsync();
    }

}
