using System.Threading.Tasks;
using LibraryIS.CommonLayer.Exceptions;
using LibraryIS.DAL;
using LibraryIS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using static System.StringComparison;

namespace LibraryIS.BLL.Helpers
{
    internal class RetrievingFromDbHelper
    {
        internal static async Task<Book> RetrieveBookFromDbAsync(LibraryDbContext dbContext, int bookId)
        {
            var book = await dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                throw new ItemNotFoundException("Book is not found");
            }

            return book;
        }

        internal static async Task<Member> RetrieveMemberFromDbAsync(LibraryDbContext dbContext, int memberId)
        {
            var member = await dbContext.Members
                .Include(m => m.MemberBooks)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member == null)
            {
                throw new ItemNotFoundException("Member is not found");
            }

            return member;
        }

        internal static async Task<User> RetrieveUserFromDbAsync(LibraryDbContext dbContext, int userId)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ItemNotFoundException("User is not found");
            }

            return user;
        }

        internal static async Task<User> RetrieveUserFromDbByCredentialsAsync(LibraryDbContext dbContext,
            string userEmail, string userPassword)
        {

            var retrievedUser = await dbContext.Users.FirstOrDefaultAsync(
                u => u.Email.Equals(userEmail, InvariantCultureIgnoreCase) &&
                     u.Password.Equals(userPassword));

            // return null if user not found
            if (retrievedUser == null)
            {
                throw new ItemNotFoundException("Incorrect email or password");
            }

            return retrievedUser;
        }
    }
}
