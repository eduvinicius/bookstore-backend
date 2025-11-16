using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Services
{
    public class BookcasesService(IBookcaseRepository bookcaseRepository, IMapper mapper, IBookRepository bookRepository) : IBookcasesService
    {
        private readonly IBookcaseRepository _bookcaseRepository = bookcaseRepository;
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<Bookcase>> GetAllBookcasesAsync(int page, int pageSize)
        {
            return await _bookcaseRepository.GetAllAsync(page, pageSize);
        }

        public async Task<Bookcase> CreateBookcaseAsync(CreateBookcaseDto dto)
        {
            var bookcase = _mapper.Map<Bookcase>(dto);

            await _bookcaseRepository.AddAsync(bookcase);
            await _bookcaseRepository.SaveChangesAsync();

            if (dto.BookIds?.Count > 0)
            {
                foreach (var bookId in dto.BookIds)
                {
                    var book = await _bookRepository.GetByIdAsync(bookId) ?? throw new Exception($"Book {bookId} not found.");

                    if (book.BookcaseId != null)
                        throw new Exception($"Book {bookId} is already in another bookcase.");

                    book.BookcaseId = bookcase.Id;
                    _bookRepository.Update(book);
                }

                await _bookRepository.SaveChangesAsync();
            }

            return bookcase;
        }

        public async Task<Bookcase> GetBookcaseByIdAsync(int id)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(id);

            return bookcase ?? throw new KeyNotFoundException($"Bookcase with ID {id} not found.");
        }

        public async Task<Bookcase> UpdateBookcaseAsync(UpdateBookcaseDto dto)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException($"Book with ID {dto.Id} not found.");


            _mapper.Map(dto, bookcase);

            var currentBookIds = bookcase.Books.Select(b => b.Id).ToList();
            var booksToAddIds = dto.BookIds.Except(currentBookIds).ToList();
            var booksToRemoveIds = currentBookIds.Except(dto.BookIds).ToList();

            if (booksToAddIds.Count != 0)
            {
                var booksToAdd = await _bookRepository.GetByIdsListAsync(booksToAddIds);

                foreach (var book in booksToAdd)
                    book.BookcaseId = bookcase.Id;
            }

            if (booksToRemoveIds.Count != 0)
            {
                var booksToRemove = await _bookRepository.GetByIdsListAsync(booksToRemoveIds);

                foreach (var book in booksToRemove)
                    book.BookcaseId = null;
            }


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

            var books = await _bookRepository.GetByBookcaseIdAsync(id);

            foreach (var book in books)
                book.BookcaseId = null;

            await _bookRepository.SaveChangesAsync();

            _bookcaseRepository.Delete(bookcase);
            await _bookcaseRepository.SaveChangesAsync();

            return true;
        }
    }
}
