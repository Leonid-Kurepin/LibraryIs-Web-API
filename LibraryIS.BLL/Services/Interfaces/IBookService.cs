using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.FilteringModels;

namespace LibraryIS.BLL.Services.Interfaces
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetFilteredBooks(BooksFilterModel filterModel);
        Task<BookDto> GetBookByIdAsync(int bookId);
        Task<BookDto> AddBookAsync(BookDto bookDto, UserDto userDto);
        Task<BookDto> UpdateBookAsync(int bookId, BookDto newBookInfo);
        Task<BookDto> GiveBookToMemberAsync(int bookId, int memberId, UserDto userDto);
        Task<BookDto> TakeBookFromMemberAsync(int bookId, int memberId, UserDto userDto);
        Task<BookDto> SetBookAvailableCountAsync(int bookId, int count);
        Task<BookDto> ChangeBookAvailableCountAsync(int bookId, int countChange);
        Task<bool> DeleteBookAsync(int bookId);
    }
}
