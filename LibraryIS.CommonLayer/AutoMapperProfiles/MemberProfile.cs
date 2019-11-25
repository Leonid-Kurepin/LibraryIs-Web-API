using AutoMapper;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.DAL.Entities;

namespace LibraryIS.CommonLayer.AutoMapperProfiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>()
                .ForMember(
                    dest => dest.CountOfBooks,
                    opt => opt.MapFrom(src => src.MemberBooks.Count));

            CreateMap<MemberDto, Member>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
