using AutoMapper;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.DAL.Entities;

namespace LibraryIS.CommonLayer.AutoMapperProfiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<Publisher, PublisherDto>();

            CreateMap<PublisherDto, Publisher>();
        }
    }
}
