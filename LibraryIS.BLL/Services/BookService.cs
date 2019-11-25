using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryIS.BLL.Services.Interfaces;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;
using LibraryIS.DAL;
using LibraryIS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.String;
using static System.StringComparison;
using static LibraryIS.BLL.Validators.BookValidator;
using static LibraryIS.BLL.Helpers.BusinessLogicHelper;
using static LibraryIS.BLL.Helpers.RetrievingFromDbHelper;
using static LibraryIS.BLL.Helpers.UniquenessInDbCheckerHelper;
using static LibraryIS.BLL.Logging.LogMessagesProvider;

namespace LibraryIS.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BookService(LibraryDbContext libraryDbContext, IMapper mapper, ILogger<BookService> logger)
        {
            _db = libraryDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<BookDto> GetFilteredBooks(BooksFilterModel filterModel)
        {
            //Filtering logic  
            var books = _db.Books.Include(b => b.Author)
                .Include(b => b.Publisher)
                .Where(b => b.Title.StartsWith(filterModel.Title ??
                                               Empty, InvariantCultureIgnoreCase))
                .Where(b => b.Author.Name.StartsWith(filterModel.AuthorName ??
                                                     Empty, InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filterModel.Limit)
                .Take(filterModel.Limit);

            return _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookByIdAsync(int bookId)
        {
            var retrievedBook = await RetrieveBookFromDbAsync(_db, bookId);

            return _mapper.Map<Book, BookDto>(retrievedBook);
        }

        public async Task<BookDto> AddBookAsync(BookDto bookDto, UserDto userDto)
        {
            ValidateBookTitle(bookDto.Title);
            ValidateDateOfPublishing(bookDto.DateOfPublishing);
            ValidateIsbn(bookDto.Isbn);
            ValidateSummary(bookDto.Summary);
            ValidateAvailableCount(bookDto.CountAvailable);

            await CheckAuthorInDbAsync(_db, _mapper, bookDto);
            await CheckPublisherInDbAsync(_db, _mapper, bookDto);

            var book = _mapper.Map<BookDto, Book>(bookDto);

            await _db.AddAsync(book);
            await _db.SaveChangesAsync();

            _logger.LogInformation(AddBookToLibraryLogMessage(book, userDto));

            return _mapper.Map<Book, BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(int bookId, BookDto newBookInfo)
        {
            var updatedBook = await RetrieveBookFromDbAsync(_db, bookId);

            ValidateBookTitle(newBookInfo.Title, false);
            ValidateDateOfPublishing(newBookInfo.DateOfPublishing);
            ValidateIsbn(newBookInfo.Isbn);
            ValidateSummary(newBookInfo.Summary);
            ValidateAvailableCount(newBookInfo.CountAvailable);

            await CheckAuthorInDbAsync(_db, _mapper, newBookInfo);
            await CheckPublisherInDbAsync(_db, _mapper, newBookInfo);

            _mapper.Map<BookDto, Book>(newBookInfo, updatedBook);

            _db.Update(updatedBook);
            await _db.SaveChangesAsync();

            return _mapper.Map<Book, BookDto>(updatedBook);
        }

        public async Task<BookDto> GiveBookToMemberAsync(int bookId, int memberId, UserDto userDto)
        {
            var retrievedBook = await RetrieveBookFromDbAsync(_db, bookId);

            var retrievedMember = await RetrieveMemberFromDbAsync(_db, memberId);

            CheckBookIsAvailable(retrievedBook);
            CheckMemberNotInBlacklist(retrievedMember);
            CheckMemberHasAllowedAmountOfBooks(retrievedMember);
            CheckMemberHasSuchBook(retrievedMember, bookId);

            retrievedBook.CountAvailable--;

            await _db.MemberBooks.AddAsync(new MemberBook(memberId, bookId));

            _db.Books.Update(retrievedBook);
            await _db.SaveChangesAsync();

            _logger.LogInformation(GiveBookToMemberLogMessage(retrievedBook, retrievedMember, userDto));

            return _mapper.Map<Book, BookDto>(retrievedBook);
        }

        public async Task<BookDto> TakeBookFromMemberAsync(int bookId, int memberId, UserDto userDto)
        {
            var retrievedBook = await RetrieveBookFromDbAsync(_db, bookId);
            var retrievedMember = await RetrieveMemberFromDbAsync(_db, memberId);

            CheckMemberHasSuchBook(retrievedMember, bookId, false);

            var memberBookRecord = retrievedMember.MemberBooks.FirstOrDefault(mb => mb.BookId == bookId);
            _db.MemberBooks.Remove(memberBookRecord);

            retrievedBook.CountAvailable++;

            // If member returned all books then delete him from blacklist
            if (retrievedMember.MemberBooks.Count - 1 == 0)
            {
                retrievedMember.IsInBlacklist = false;
                _db.Members.Update(retrievedMember);
            }

            _db.Books.Update(retrievedBook);
            await _db.SaveChangesAsync();

            _logger.LogInformation(TakeBookFromMemberLogMessage(retrievedBook, retrievedMember, userDto));

            return _mapper.Map<Book, BookDto>(retrievedBook);
        }

        public async Task<BookDto> SetBookAvailableCountAsync(int bookId, int count)
        {
            var retrievedBook = await RetrieveBookFromDbAsync(_db, bookId);

            ValidateAvailableCount(count);

            retrievedBook.CountAvailable = count;

            _db.Books.Update(retrievedBook);
            await _db.SaveChangesAsync();

            return _mapper.Map<Book, BookDto>(retrievedBook);
        }

        public async Task<BookDto> ChangeBookAvailableCountAsync(int bookId, int countChange)
        {
            var retrievedBook = await RetrieveBookFromDbAsync(_db, bookId);

            var newAvailableCount = retrievedBook.CountAvailable + countChange;

            ValidateAvailableCount(newAvailableCount);

            retrievedBook.CountAvailable = newAvailableCount;

            _db.Books.Update(retrievedBook);
            await _db.SaveChangesAsync();

            return _mapper.Map<Book, BookDto>(retrievedBook);
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var retrievedBook = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            if (retrievedBook == null)
            {
                return false;
            }

            _db.Books.Remove(retrievedBook);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
