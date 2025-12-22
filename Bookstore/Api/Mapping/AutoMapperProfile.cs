using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.DTOs.External;
using Bookstore.Domain.Entities;

namespace Bookstore.Api.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<Book, BookDto>();

            CreateMap<Bookcase, BookcaseDto>();

            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateBookcaseDto, Bookcase>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateBookcaseDto, Bookcase>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<GoogleBookDto, CreateBookDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Author,
                    opt => opt.MapFrom(src => string.Join(", ", src.Authors)))
                .ForMember(dest => dest.Year,
                    opt => opt.MapFrom(src => src.PublishedYear ?? 0))
                .ForMember(dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.Thumbnail))
                .ForMember(dest => dest.Genre,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsRead,
                    opt => opt.MapFrom(_ => false));

        }
    }
}

