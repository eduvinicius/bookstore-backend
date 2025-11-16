using Bookstore.Api.Models;

namespace Bookstore.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book?> GetByIdAsync(int id);
        Task<List<Book>> GetByIdsListAsync(List<int> ids);
        Task<IEnumerable<Book>> GetAllAsync(int skip, int take);
        Task<IEnumerable<Book>> GetByBookcaseIdAsync(int id);
        Task<IEnumerable<Book>> GetUnassignedBooksAsync();
        void Update(Book book);
        void Delete(Book book);
        Task SaveChangesAsync();
    }
}
