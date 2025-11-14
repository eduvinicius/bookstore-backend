using Bookstore.Api.Models;

namespace Bookstore.Repositories.Interfaces
{
    public interface IBookcaseRepository
    {
        Task AddAsync(Bookcase bookcase);
        Task<Bookcase?> GetByIdAsync(int id);
        Task<IEnumerable<Bookcase>> GetAllAsync(int skip, int take);
        void Update(Bookcase bookcase);
        void Delete(Bookcase bookcase);
        Task SaveChangesAsync();
    }
}
