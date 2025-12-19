using Bookstore.Api.DTOs.External;

namespace Bookstore.App.Services.Interfaces
{
    public interface IGoogleBooksService
    {
        Task<IEnumerable<GoogleBookDto>> SearchAsync(string query, int maxResults = 10, int startIndex = 0);
    }
}
