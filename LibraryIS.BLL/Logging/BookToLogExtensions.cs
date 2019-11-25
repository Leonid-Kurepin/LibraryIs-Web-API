using System;
using System.Text;
using LibraryIS.DAL.Entities;

namespace LibraryIS.BLL.Logging
{
    internal static class BookToLogExtensions
    {
        internal static StringBuilder ToLogString(this Book book)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Title: " + book.Title);
            stringBuilder.AppendLine();

            if (book.Author != null)
            {
                stringBuilder.Append("Author: " + book.Author.Name);
                stringBuilder.AppendLine();
            }

            if (book.Publisher != null)
            {
                stringBuilder.Append("Publisher: " + book.Publisher.Name);
                stringBuilder.AppendLine();
            }

            if (book.DateOfPublishing != null)
            {
                var dateOfPublishing = (DateTime) book.DateOfPublishing;
                stringBuilder.Append("Year of publishing: " + dateOfPublishing.Year);
                stringBuilder.AppendLine();
            }

            if (book.Isbn != null)
            {
                stringBuilder.Append("ISBN: " + book.Isbn);
                stringBuilder.AppendLine();
            }

            stringBuilder.Append("Available count: " + book.CountAvailable);

            return stringBuilder;
        }
    }
}
