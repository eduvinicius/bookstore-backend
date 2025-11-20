using Bookstore.Api.DTOs;
using Bookstore.Api.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateBookAsync(CreateBookDto dto);
        Task<Book> UpdateBookAsync(UpdateBookDto dto);
        Task<bool> DeleteBookAsync(int id);
        Task<BookDto> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetAllBooksAsync(int page, int pageSize);
        Task<IEnumerable<BookDto>> GetUnassignedBooksAsync();
    }
}
