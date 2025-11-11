using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.Models;

namespace Bookstore.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // CreateBookDto -> Book
            CreateMap<CreateBookDto, Book>()
                // set CreatedDate/UpdatedDate automatically at mapping time
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Book -> BookDto
            CreateMap<Book, BookDto>();

            // (Optional) If you need reverse mapping for updates:
            CreateMap<CreateBookDto, Book>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}

