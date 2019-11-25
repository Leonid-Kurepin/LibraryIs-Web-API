using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryIS.BLL.Services.Interfaces;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;
using LibraryIS.WebApplication.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static System.String;

namespace LibraryIS.WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET api/books  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<PagedCollectionResponse<BookDto>> GetBooks([FromQuery] BooksFilterModel filter)
        {
            //Get the data for the current page  
            var result = new PagedCollectionResponse<BookDto>
            {
                Items = _bookService.GetFilteredBooks(filter)
            };

            if (!result.Items.Any())
            {
                return NoContent();
            }

            //Get next page URL string  
            BooksFilterModel nextFilter = filter.Clone() as BooksFilterModel;
            nextFilter.Page += 1;
            string nextUrl = !_bookService.GetFilteredBooks(nextFilter).Any()
                ? null
                : Url.Action("GetBooks", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            BooksFilterModel previousFilter = filter.Clone() as BooksFilterModel;
            previousFilter.Page -= 1;
            string previousUrl = previousFilter.Page <= 0
                ? null
                : Url.Action("GetBooks", null, previousFilter, Request.Scheme);

            result.NextPage = !IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return Ok(result);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBookById([FromRoute] int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            return Ok(book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> EditBook([FromRoute] int id, [FromBody] BookDto bookDto)
        {
            var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);

            return Ok(updatedBook);
        }

        // PUT: api/Books/5/out?memberId=2
        [HttpPut("{id}/out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookDto>> GiveBookToMember([FromRoute] int id, [FromQuery] int memberId)
        {
            var authorizedUser = this.GetAuthorizedUser();

            return Ok(await _bookService.GiveBookToMemberAsync(id, memberId, authorizedUser));
        }

        // PUT: api/Books/5/in?memberId=2
        [HttpPut("{id}/in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookDto>> TakeBookFromMember([FromRoute] int id, [FromQuery] int memberId)
        {
            var authorizedUser = this.GetAuthorizedUser();

            return Ok(await _bookService.TakeBookFromMemberAsync(id, memberId, authorizedUser));
        }

        // PUT: api/Books/5/set-book-available-count?count=4
        [HttpPut("{id}/set-book-available-count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> SetBookAvailableCount([FromRoute] int id, [FromQuery] int count)
        {
            return Ok(await _bookService.SetBookAvailableCountAsync(id, count));
        }

        // PUT: api/Books/5/change-book-available-count?countChange=-2
        [HttpPut("{id}/change-book-available-count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> ChangeBookAvailableCount([FromRoute] int id,
            [FromQuery] int countChange)
        {
            return Ok(await _bookService.ChangeBookAvailableCountAsync(id, countChange));
        }

        // POST: api/Books
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BookDto>> AddBook([FromBody] BookDto bookDto)
        {
            var authorizedUser = this.GetAuthorizedUser();

            return CreatedAtAction(nameof(AddBook),
                await _bookService.AddBookAsync(bookDto, authorizedUser));
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> DeleteBook([FromRoute] int id)
        {
            return await _bookService.DeleteBookAsync(id);
        }
    }
}
