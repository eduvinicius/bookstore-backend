using Bookstore.Api.Models;

namespace Bookstore.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync(int skip, int take);
        void Update(Book book);
        void Delete(Book book);
        Task SaveChangesAsync();
    }
}
