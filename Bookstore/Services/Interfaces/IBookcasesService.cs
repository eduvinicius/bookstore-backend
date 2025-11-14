using Bookstore.Api.DTOs;
using Bookstore.Api.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IBookcasesService
    {
        Task<Bookcase> CreateBookcaseAsync(CreateBookcaseDto dto);
        Task<Bookcase> UpdateBookcaseAsync(UpdateBookcaseDto dto);
        Task<bool> DeleteBookcaseAsync(int id);
        Task<Bookcase> GetBookcaseByIdAsync(int id);
        Task<IEnumerable<Bookcase>> GetAllBookcasesAsync(int page, int pageSize);
    }
}
