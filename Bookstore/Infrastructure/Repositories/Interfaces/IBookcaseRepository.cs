using Bookstore.App.Filters;
using Bookstore.Domain.Entities;

namespace Bookstore.Infrastructure.Repositories.Interfaces
{
    public interface IBookcaseRepository: IRepository<Bookcase>
    {
        Task<IEnumerable<Bookcase>> GetAllAsync(BookcaseFilter filter);
    }
}
