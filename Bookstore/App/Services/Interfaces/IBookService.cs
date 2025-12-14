using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.Domain.Entities;

namespace Bookstore.App.Services.Interfaces
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
