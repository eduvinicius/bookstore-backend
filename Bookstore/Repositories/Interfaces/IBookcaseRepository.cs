using Bookstore.Api.Models;
using Bookstore.App.Filters;

namespace Bookstore.Repositories.Interfaces
{
    public interface IBookcaseRepository
    {
        Task AddAsync(Bookcase bookcase);
        Task<Bookcase?> GetByIdAsync(int id);
        Task<IEnumerable<Bookcase>> GetAllAsync(BookcaseFilter filter);
        void Update(Bookcase bookcase);
        void Delete(Bookcase bookcase);
        Task SaveChangesAsync();
    }
}
