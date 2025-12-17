using Bookstore.Domain.Entities;

namespace Bookstore.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
