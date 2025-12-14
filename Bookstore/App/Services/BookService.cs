using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Repositories.Interfaces;

namespace Bookstore.App.Services
{
    public class BookService( IMapper mapper, IUnitOfWork unitOfWork) : IBookService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(BookFilter filter)
        {
            var books = await _unitOfWork.Books.GetAllAsync(filter);
            return books.Select(book => _mapper.Map<BookDto>(book));
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Book with ID {id} not found."); ;

            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetUnassignedBooksAsync()
        {
            var books = await _unitOfWork.Books.GetUnassignedBooksAsync();
            return books.Select(book => _mapper.Map<BookDto>(book));
        }

        public async Task<Book> CreateBookAsync(CreateBookDto dto)
        {
            var book = new Book();
            _mapper.Map(dto, book);

            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return book;
        }

        public async Task<Book> UpdateBookAsync(UpdateBookDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException($"Book with ID {dto.Id} not found.");
                
            _mapper.Map(dto, book);
            _unitOfWork.Books.Update(book);
            await _unitOfWork.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return false;
            
            _unitOfWork.Books.Delete(book);
            await _unitOfWork.Books.SaveChangesAsync();
            return true;
        }


    }
}
