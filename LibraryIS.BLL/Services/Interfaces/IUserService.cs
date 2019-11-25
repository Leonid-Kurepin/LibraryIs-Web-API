using System.Collections.Generic;
using LibraryIS.CommonLayer.DTO;
using System.Threading.Tasks;
using LibraryIS.CommonLayer.FilteringModels;

namespace LibraryIS.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateAsync(UserDto userDto);
        IEnumerable<UserDto> GetFilteredUsers(UsersFilterModel filterModel);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> AddUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(int userId, UserDto newUserInfo);
        Task<bool> DeleteUserAsync(int userId);
    }
}
