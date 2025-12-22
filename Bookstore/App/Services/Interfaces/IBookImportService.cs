using Bookstore.Domain.Entities;

namespace Bookstore.App.Services.Interfaces
{
    public interface IBookImportService
    {
        Task<Book> ImportFromGoogleAsync(string googleBookId);
    }
}
