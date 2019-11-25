using AutoMapper;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.DAL.Entities;

namespace LibraryIS.CommonLayer.AutoMapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(src => src.Author))
                .ForMember(
                    dest => dest.Publisher,
                    opt => opt.MapFrom(src => src.Publisher));

            CreateMap<BookDto, Book>()
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(src => src.Author))
                .ForMember(
                    dest => dest.Publisher,
                    opt => opt.MapFrom(src => src.Publisher))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
