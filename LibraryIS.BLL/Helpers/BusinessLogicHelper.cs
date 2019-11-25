using System.Linq;
using LibraryIS.CommonLayer.Exceptions;
using LibraryIS.DAL.Entities;

namespace LibraryIS.BLL.Helpers
{
    internal class BusinessLogicHelper
    {
        internal static void CheckBookIsAvailable(Book book)
        {
            if (book.CountAvailable == 0)
            {
                throw new ResourceConflictException(
                    "There is no available copy of this book in the library at this moment");
            }
        }

        internal static void CheckMemberNotInBlacklist(Member member)
        {
            if ((bool) member.IsInBlacklist)
            {
                throw new ResourceConflictException("This member cant\'t take any book. " +
                                                    "He is in the library blacklist at this moment");
            }
        }

        internal static void CheckMemberHasAllowedAmountOfBooks(Member retrievedMember)
        {
            const int allowedAmountOfBooks = 5;

            if (retrievedMember.MemberBooks.Count >= allowedAmountOfBooks)
            {
                throw new ResourceConflictException("This member cant\'t take any book. " +
                                                    "A member mustn't have more than" + allowedAmountOfBooks +
                                                    " books at a time");
            }
        }

        internal static void CheckMemberHasSuchBook(Member member, int bookId, bool fromLibraryToMember = true)
        {
            var isMemberHaveBook = member.MemberBooks.Any(mb => mb.BookId == bookId);

            if (fromLibraryToMember)
            {
                if (isMemberHaveBook)
                {
                    throw new ResourceConflictException("This member cant\'t take this book. " +
                                                        "He already has one copy of this book");
                }

                return;
            }

            if (!isMemberHaveBook)
            {
                throw new ResourceConflictException("This member cant\'t return this book back to the library. " +
                                                    "He don\'t have any copy of this book");
            }
        }
    }
}

