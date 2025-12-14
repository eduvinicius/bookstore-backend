namespace Bookstore.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IBookRepository Books { get; }
        IBookcaseRepository Bookcases { get; }

        Task<int> SaveChangesAsync();
    }
}
