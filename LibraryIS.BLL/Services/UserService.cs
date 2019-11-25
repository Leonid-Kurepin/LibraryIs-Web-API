using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryIS.Auth;
using LibraryIS.BLL.Services.Interfaces;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;
using LibraryIS.DAL;
using LibraryIS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static System.String;
using static System.StringComparison;
using static LibraryIS.BLL.Helpers.RetrievingFromDbHelper;
using static LibraryIS.BLL.Helpers.UniquenessInDbCheckerHelper;
using static LibraryIS.BLL.Validators.UserValidator;

namespace LibraryIS.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly LibraryDbContext _db;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(LibraryDbContext libraryDbContext, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _db = libraryDbContext;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<UserDto> AuthenticateAsync(UserDto userDto)
        {
            var retrievedUser = await RetrieveUserFromDbByCredentialsAsync(_db, userDto.Email, userDto.Password);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, retrievedUser.Id.ToString()),
                    new Claim(ClaimTypes.GivenName, retrievedUser.Name),
                    new Claim(ClaimTypes.Email, retrievedUser.Email),
                    new Claim(ClaimTypes.Role, retrievedUser.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<User, UserDto>(retrievedUser);
            user.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<UserDto> GetFilteredUsers(UsersFilterModel filterModel)
        {
            //Filtering logic  
            var users = _db.Users
                .Where(u => u.Name.StartsWith(filterModel.Name ??
                                              Empty, InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filterModel.Limit)
                .Take(filterModel.Limit);

            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        //public async Task<IEnumerable<UserDto>> GetUsersAsync()
        //{
        //    var retrievedUsers = await _db.Users.ToListAsync();

        //    return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(retrievedUsers);
        //}

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var retrievedUser = await RetrieveUserFromDbAsync(_db, userId);

            return _mapper.Map<User, UserDto>(retrievedUser);
        }

        public async Task<UserDto> AddUserAsync(UserDto userDto)
        {
            ValidateUserName(userDto.Name);
            ValidateUserEmail(userDto.Email);
            ValidateUserPassword(userDto.Password);

            await CheckUserInDbAsync(_db, userDto);

            var user = _mapper.Map<UserDto, User>(userDto);

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UserDto newUserInfo)
        {
            var updatedUser = await RetrieveUserFromDbAsync(_db, userId);

            ValidateUserName(newUserInfo.Name, false);
            ValidateUserEmail(newUserInfo.Email, false);
            ValidateUserPassword(newUserInfo.Password, false);


            if (!updatedUser.Email.Equals(newUserInfo.Email, InvariantCultureIgnoreCase))
            {
                await CheckUserInDbAsync(_db, newUserInfo);
            }

            _mapper.Map<UserDto, User>(newUserInfo, updatedUser);

            _db.Users.Update(updatedUser);
            await _db.SaveChangesAsync();

            return _mapper.Map<User, UserDto>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var retrievedUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (retrievedUser == null)
            {
                return false;
            }

            _db.Users.Remove(retrievedUser);
            await _db.SaveChangesAsync();

            return true;
        }

    }
}
