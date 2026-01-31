using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.App.Filters;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Exceptions;
using Bookstore.Infrastructure.Repositories.Interfaces;

namespace Bookstore.App.Services
{
    public class BookcasesService(IMapper mapper, IUnitOfWork unitOfWork) : IBookcasesService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<BookcaseDto>> GetAllBookcasesAsync(BookcaseFilter filter)
        {
            var books = await _unitOfWork.Bookcases.GetAllAsync(filter);
            return books.Select(bookcase => _mapper.Map<BookcaseDto>(bookcase));
        }

        public async Task<BookcaseDto> GetBookcaseByIdAsync(int id)
        {
            var bookcase = await _unitOfWork.Bookcases.GetByIdAsync(id)
                ?? throw new NotFoundException("Bookcase", id);

            return _mapper.Map<BookcaseDto>(bookcase);
        }

        public async Task<Bookcase> CreateBookcaseAsync(CreateBookcaseDto dto, int userId)
        {
            var bookcase = _mapper.Map<Bookcase>(dto);
            bookcase.UserId = userId;

            if (dto.BookIds?.Count > 0)
            {
                var books = await _unitOfWork.Books.GetByIdsListAsync(dto.BookIds);

                foreach (var book in books)
                {
                    if (book.BookcaseId != null)
                        throw new ConflictException($"Book '{book.Title}' is already in another bookcase.");
                }

                bookcase.Books = books;
            }

            await _unitOfWork.Bookcases.AddAsync(bookcase);
            await _unitOfWork.SaveChangesAsync();

            return bookcase;
        }

        public async Task<Bookcase> UpdateBookcaseAsync(UpdateBookcaseDto dto)
        {
            var bookcase = await _unitOfWork.Bookcases.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException("Bookcase", dto.Id);

            _mapper.Map(dto, bookcase);

            var books = await _unitOfWork.Books.GetByIdsListAsync(dto.BookIds);

            bookcase.Books = books;

            _unitOfWork.Bookcases.Update(bookcase);
            await _unitOfWork.SaveChangesAsync();
            return bookcase;
        }

        public async Task DeleteBookcaseAsync(int id)
        {
            var bookcase = await _unitOfWork.Bookcases.GetByIdAsync(id)
                ?? throw new NotFoundException("Bookcase", id);

            _unitOfWork.Bookcases.Delete(bookcase);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
