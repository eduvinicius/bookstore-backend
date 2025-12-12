using Bookstore.Api.Models;
using Bookstore.App.Filters;

namespace Bookstore.Repositories.Interfaces
{
    public interface IBookcaseRepository: IRepository<Bookcase>
    {
        Task<IEnumerable<Bookcase>> GetAllAsync(BookcaseFilter filter);
    }
}
