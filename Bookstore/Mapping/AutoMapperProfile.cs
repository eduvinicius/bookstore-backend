using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.Models;

namespace Bookstore.Mapping
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


        }
    }
}

