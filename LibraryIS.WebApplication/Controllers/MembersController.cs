using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryIS.BLL.Services.Interfaces;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;
using LibraryIS.WebApplication.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace LibraryIS.WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: api/Members
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<PagedCollectionResponse<MemberDto>> GetMembers([FromQuery] MembersFilterModel filter)
        {
            //Get the data for the current page  
            var result = new PagedCollectionResponse<MemberDto>
            {
                Items = _memberService.GetFilteredMembers(filter)
            };

            if (!result.Items.Any())
            {
                return NoContent();
            }

            //Get next page URL string  
            MembersFilterModel nextFilter = filter.Clone() as MembersFilterModel;
            nextFilter.Page += 1;
            string nextUrl = !_memberService.GetFilteredMembers(nextFilter).Any()
                ? null
                : Url.Action("GetMembers", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            MembersFilterModel previousFilter = filter.Clone() as MembersFilterModel;
            previousFilter.Page -= 1;
            string previousUrl = previousFilter.Page <= 0
                ? null
                : Url.Action("GetMembers", null, previousFilter, Request.Scheme);

            result.NextPage = !IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return Ok(result);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> GetMemberById([FromRoute] int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // PUT: api/Members/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<MemberDto>> EditMember([FromRoute] int id, [FromBody] MemberDto memberDto)
        {
            var updatedMember = await _memberService.UpdateMemberAsync(id, memberDto);

            return Ok(updatedMember);
        }

        // PUT: api/Members/5/add-to-blacklist
        [HttpPut("{id}/add-to-blacklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> AddMemberToBlackList([FromRoute] int id)
        {
            return Ok(await _memberService.AddMemberToBlacklistAsync(id));
        }

        // PUT: api/Members/5/remove-from-blacklist
        [HttpPut("{id}/remove-from-blacklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> RemoveMemberFromBlackList([FromRoute] int id)
        {
            return Ok(await _memberService.RemoveMemberFromBlacklistAsync(id));
        }

        // POST: api/Members
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<MemberDto>> AddMember([FromBody] MemberDto memberDto)
        {
            var authorizedUser = this.GetAuthorizedUser();

            return CreatedAtAction(nameof(AddMember),
                await _memberService.AddMemberAsync(memberDto, authorizedUser));
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteMember([FromRoute] int id)
        {

            return Ok(await _memberService.DeleteMemberAsync(id));
        }
    }
}
