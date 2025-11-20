using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;

namespace Bookstore.Services
{
    public class BookcasesService(IBookcaseRepository bookcaseRepository, IMapper mapper, IBookRepository bookRepository) : IBookcasesService
    {
        private readonly IBookcaseRepository _bookcaseRepository = bookcaseRepository;
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<BookcaseDto>> GetAllBookcasesAsync(int page, int pageSize)
        {
            var books = await _bookcaseRepository.GetAllAsync(page, pageSize);
            return books.Select(bookcase => _mapper.Map<BookcaseDto>(bookcase));
        }

        public async Task<Bookcase> CreateBookcaseAsync(CreateBookcaseDto dto)
        {
            var bookcase = _mapper.Map<Bookcase>(dto);

            if (dto.BookIds?.Count > 0)
            {
                var books = await _bookRepository.GetByIdsListAsync(dto.BookIds);

                foreach (var book in books)
                {

                    if (book.BookcaseId != null)
                        throw new Exception($"Book {book.Id} is already in another bookcase.");
                }

                bookcase.Books = books;
            }

            await _bookcaseRepository.AddAsync(bookcase);
            await _bookcaseRepository.SaveChangesAsync();

            return bookcase;
        }

        public async Task<BookcaseDto> GetBookcaseByIdAsync(int id)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(id);

            return _mapper.Map<BookcaseDto>(bookcase) ?? throw new KeyNotFoundException($"Bookcase with ID {id} not found.");
        }

        public async Task<Bookcase> UpdateBookcaseAsync(UpdateBookcaseDto dto)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException($"Book with ID {dto.Id} not found.");

            _mapper.Map(dto, bookcase);

            var books = await _bookRepository.GetByIdsListAsync(dto.BookIds);

            bookcase.Books = books;

            _bookcaseRepository.Update(bookcase);
            await _bookcaseRepository.SaveChangesAsync();
            return bookcase;
        }

        public async Task<bool> DeleteBookcaseAsync(int id)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(id);

            if (bookcase == null)
            {
                return false;
            }

            _bookcaseRepository.Delete(bookcase);
            await _bookcaseRepository.SaveChangesAsync();

            return true;
        }
    }
}
