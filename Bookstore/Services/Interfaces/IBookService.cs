using Bookstore.Api.DTOs;
using Bookstore.Api.Models;
using Bookstore.App.Filters;

namespace Bookstore.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateBookAsync(CreateBookDto dto);
        Task<Book> UpdateBookAsync(UpdateBookDto dto);
        Task<bool> DeleteBookAsync(int id);
        Task<BookDto> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetAllBooksAsync(BookFilter filter);
        Task<IEnumerable<BookDto>> GetUnassignedBooksAsync();
    }
}
