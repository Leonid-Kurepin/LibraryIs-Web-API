using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;

namespace LibraryIS.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberDto> GetFilteredMembers(MembersFilterModel filterModel);
        Task<MemberDto> GetMemberByIdAsync(int memberId);
        Task<MemberDto> AddMemberAsync(MemberDto memberDto, UserDto userDto);
        Task<MemberDto> UpdateMemberAsync(int memberId, MemberDto newMemberInfo);
        Task<MemberDto> AddMemberToBlacklistAsync(int memberId);
        Task<MemberDto> RemoveMemberFromBlacklistAsync(int memberId);
        Task<bool> DeleteMemberAsync(int memberId);
    }
}
