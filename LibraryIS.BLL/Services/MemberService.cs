using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryIS.BLL.Logging;
using LibraryIS.BLL.Services.Interfaces;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.Exceptions;
using LibraryIS.CommonLayer.FilteringModels;
using LibraryIS.DAL;
using LibraryIS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.String;
using static System.StringComparison;
using static LibraryIS.BLL.Helpers.RetrievingFromDbHelper;
using static LibraryIS.BLL.Helpers.UniquenessInDbCheckerHelper;
using static LibraryIS.BLL.Validators.MemberValidator;

namespace LibraryIS.BLL.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MemberService(LibraryDbContext libraryDbContext, IMapper mapper, ILogger<MemberService> logger)
        {
            _db = libraryDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<MemberDto> GetFilteredMembers(MembersFilterModel filterModel)
        {
            //Filtering logic  
            var members = _db.Members
                .Where(m => m.Name.StartsWith(filterModel.Name ??
                                              Empty, InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filterModel.Limit)
                .Take(filterModel.Limit);

            return _mapper.Map<IEnumerable<Member>, IEnumerable<MemberDto>>(members);
        }

        public async Task<MemberDto> GetMemberByIdAsync(int memberId)
        {
            var retrievedMember = await RetrieveMemberFromDbAsync(_db, memberId);

            return _mapper.Map<Member, MemberDto>(retrievedMember);
        }

        public async Task<MemberDto> AddMemberAsync(MemberDto memberDto, UserDto userDto)
        {
            ValidateMemberName(memberDto.Name);
            ValidateMemberPassportNumber(memberDto.PassportNumber);
            ValidateMemberPassportSeries(memberDto.PassportSeries);
            ValidateMemberBirthDate(memberDto.DateOfBirth);

            await CheckMemberInDbAsync(_db, memberDto);

            var member = _mapper.Map<MemberDto, Member>(memberDto);

            await _db.Members.AddAsync(member);
            await _db.SaveChangesAsync();

            _logger.LogInformation(LogMessagesProvider.AddMemberToLibraryLogMessage(member, userDto));

            return _mapper.Map<Member, MemberDto>(member);
        }

        public async Task<MemberDto> UpdateMemberAsync(int memberId, MemberDto newMemberInfo)
        {
            var updatedMember = await RetrieveMemberFromDbAsync(_db, memberId);

            ValidateMemberName(newMemberInfo.Name);
            ValidateMemberPassportNumber(newMemberInfo.PassportNumber);
            ValidateMemberPassportSeries(newMemberInfo.PassportSeries);
            ValidateMemberBirthDate(newMemberInfo.DateOfBirth);

            if (updatedMember.PassportNumber != newMemberInfo.PassportNumber ||
                updatedMember.PassportSeries != newMemberInfo.PassportSeries)
            {
                await CheckMemberInDbAsync(_db, newMemberInfo);
            }

            _mapper.Map<MemberDto, Member>(newMemberInfo, updatedMember);

            _db.Update(updatedMember);
            await _db.SaveChangesAsync();

            return _mapper.Map<Member, MemberDto>(updatedMember);
        }

        public async Task<MemberDto> AddMemberToBlacklistAsync(int memberId)
        {
            var retrievedMember = await RetrieveMemberFromDbAsync(_db, memberId);

            retrievedMember.IsInBlacklist = true;

            _db.Members.Update(retrievedMember);
            await _db.SaveChangesAsync();

            return _mapper.Map<Member, MemberDto>(retrievedMember);
        }

        public async Task<MemberDto> RemoveMemberFromBlacklistAsync(int memberId)
        {
            var retrievedMember = await RetrieveMemberFromDbAsync(_db, memberId);

            retrievedMember.IsInBlacklist = false;

            _db.Members.Update(retrievedMember);
            await _db.SaveChangesAsync();

            return _mapper.Map<Member, MemberDto>(retrievedMember);
        }

        public async Task<bool> DeleteMemberAsync(int memberId)
        {
            var retrievedMember = await _db.Members
                .Include(m => m.MemberBooks)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (retrievedMember == null)
            {
                return false;
            }

            if (retrievedMember.MemberBooks.Count != 0)
            {
                throw new ResourceConflictException("This member can\'t be deleted " +
                                                    "until he has any book from the library");
            }

            _db.Members.Remove(retrievedMember);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
