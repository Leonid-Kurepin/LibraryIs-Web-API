using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryIS.Auth;
using LibraryIS.BLL.Services.Interfaces;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace LibraryIS.WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Authenticate([FromBody] UserDto userParam)
        {
            var user = await _userService.AuthenticateAsync(userParam);

            return Ok(user);
        }

        // GET api/users  
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<PagedCollectionResponse<UserDto>> GetUsers([FromQuery] UsersFilterModel filter)
        {
            //Get the data for the current page  
            var result = new PagedCollectionResponse<UserDto>
            {
                Items = _userService.GetFilteredUsers(filter)
            };

            if (!result.Items.Any())
            {
                return NoContent();
            }

            //Get next page URL string  
            UsersFilterModel nextFilter = filter.Clone() as UsersFilterModel;
            nextFilter.Page += 1;
            string nextUrl = !_userService.GetFilteredUsers(nextFilter).Any()
                ? null
                : Url.Action("GetUsers", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            UsersFilterModel previousFilter = filter.Clone() as UsersFilterModel;
            previousFilter.Page -= 1;
            string previousUrl = previousFilter.Page <= 0
                ? null
                : Url.Action("GetUsers", null, previousFilter, Request.Scheme);

            result.NextPage = !IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return Ok(result);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);

            if (id != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> EditUser([FromRoute] int id, [FromBody] UserDto userDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDto);

            return Ok(updatedUser);
        }

        // POST: api/Users
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] UserDto userDto)
        {
            return CreatedAtAction(nameof(AddUser),
                await _userService.AddUserAsync(userDto));
        }

        // DELETE: api/Users/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] int id)
        {

            return Ok(await _userService.DeleteUserAsync(id));
        }
    }
}
