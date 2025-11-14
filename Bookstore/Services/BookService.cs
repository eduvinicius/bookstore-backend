using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;

namespace Bookstore.Services
{
    public class BookService: IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(int page, int pageSize)
        {
            return await _bookRepository.GetAllAsync(page, pageSize);
        }

        public async Task<Book> CreateBookAsync(CreateBookDto dto)
        {
            var book = new Book();
            _mapper.Map(dto, book);

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            return book ?? throw new KeyNotFoundException($"Book with ID {id} not found.");
        }

        public async Task<Book> UpdateBookAsync(UpdateBookDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException($"Book with ID {dto.Id} not found.");
                

            _mapper.Map(dto, book);
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }
            _bookRepository.Delete(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }


    }
}
