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

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(int page, int pageSize)
        {
            var books = await _bookRepository.GetAllAsync(page, pageSize);
            return books.Select(book => _mapper.Map<BookDto>(book));
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Book with ID {id} not found."); ;

            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetUnassignedBooksAsync()
        {
            var books = await _bookRepository.GetUnassignedBooksAsync();
            return books.Select(book => _mapper.Map<BookDto>(book));
        }

        public async Task<Book> CreateBookAsync(CreateBookDto dto)
        {
            var book = new Book();
            _mapper.Map(dto, book);

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
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
