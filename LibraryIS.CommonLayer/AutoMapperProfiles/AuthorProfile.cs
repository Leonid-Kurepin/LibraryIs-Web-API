using AutoMapper;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.DAL.Entities;

namespace LibraryIS.CommonLayer.AutoMapperProfiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();

            CreateMap<AuthorDto, Author>();
        }
    }
}
