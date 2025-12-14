using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.Domain.Entities;

namespace Bookstore.App.Services.Interfaces
{
    public interface IBookcasesService
    {
        Task<Bookcase> CreateBookcaseAsync(CreateBookcaseDto dto);
        Task<Bookcase> UpdateBookcaseAsync(UpdateBookcaseDto dto);
        Task<bool> DeleteBookcaseAsync(int id);
        Task<BookcaseDto> GetBookcaseByIdAsync(int id);
        Task<IEnumerable<BookcaseDto>> GetAllBookcasesAsync(BookcaseFilter filter);
    }
}
