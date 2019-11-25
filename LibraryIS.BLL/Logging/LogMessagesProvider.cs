using System.Text;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.DAL.Entities;

namespace LibraryIS.BLL.Logging
{
    internal static class LogMessagesProvider
    {
        internal static string AddMemberToLibraryLogMessage(Member member, UserDto user)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine();
            message.Append("Member was added to the library by the authorized user.");
            message.AppendLine();
            message.Append("--------------------Member--------------------");
            message.AppendLine();
            message.Append(member.ToLogString());
            message.AppendLine();
            message.Append("--------------------User--------------------");
            message.AppendLine();
            message.Append("Name: " + user.Name);
            message.AppendLine();
            message.Append("Email: " + user.Email);
            message.AppendLine();
            message.Append("--------------------------------------------");

            return message.ToString();
        }

        internal static string AddBookToLibraryLogMessage(Book book, UserDto user)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine();
            message.Append("Book was added to the library by the authorized user.");
            message.AppendLine();
            message.Append("--------------------Book--------------------");
            message.AppendLine();
            message.Append(book.ToLogString());
            message.AppendLine();
            message.Append("--------------------User--------------------");
            message.AppendLine();
            message.Append("Name: " + user.Name);
            message.AppendLine();
            message.Append("Email: " + user.Email);
            message.AppendLine();
            message.Append("--------------------------------------------");

            return message.ToString();
        }

        internal static string GiveBookToMemberLogMessage(Book book, Member member, UserDto user)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine();
            message.Append("Book was given to a member by the authorized user.");
            message.AppendLine();
            message.Append("--------------------Book--------------------");
            message.AppendLine();
            message.Append(book.ToLogString());
            message.AppendLine();
            message.Append("-------------------Member-------------------");
            message.AppendLine();
            message.Append("Name: " + member.Name);
            message.AppendLine();
            message.Append("--------------------User--------------------");
            message.AppendLine();
            message.Append("Name: " + user.Name);
            message.AppendLine();
            message.Append("Email: " + user.Email);
            message.AppendLine();
            message.Append("--------------------------------------------");

            return message.ToString();
        }

        internal static string TakeBookFromMemberLogMessage(Book book, Member member, UserDto user)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine();
            message.Append("Book was taken from a member by the authorized user.");
            message.AppendLine();
            message.Append("--------------------Book--------------------");
            message.AppendLine();
            message.Append(book.ToLogString());
            message.AppendLine();
            message.Append("-------------------Member-------------------");
            message.AppendLine();
            message.Append("Name: " + member.Name);
            message.AppendLine();
            message.Append("--------------------User--------------------");
            message.AppendLine();
            message.Append("Name: " + user.Name);
            message.AppendLine();
            message.Append("Email: " + user.Email);
            message.AppendLine();
            message.Append("--------------------------------------------");

            return message.ToString();
        }
    }
}
