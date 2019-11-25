using System;
using LibraryIS.CommonLayer.Exceptions;
using static System.String;

namespace LibraryIS.BLL.Validators
{
    public static class BookValidator
    {
        public static void ValidateBookTitle(string bookTitle, bool validateNullOrEmpty = true)
        {
            if (IsNullOrEmpty(bookTitle))
            {
                if (validateNullOrEmpty)
                {
                    throw new ValidationException("Book\'s title can\'t be empty");
                }

                return;
            }

            if (bookTitle.Length > 255)
            {
                throw new ValidationException("Book title is too long. Max title\'s length is 255 symbols");
            }
        }

        public static void ValidateAuthorName(string authorName)
        {
            if (IsNullOrWhiteSpace(authorName))
            {
                return;
            }

            if (authorName.Length > 200)
            {
                throw new ValidationException("Author name is too long. Max author\'s name length is 200 symbols");
            }
        }

        public static void ValidatePublisherName(string publisherName)
        {
            if (IsNullOrWhiteSpace(publisherName))
            {
                return;
            }

            if (publisherName.Length > 255)
            {
                throw new ValidationException("Author name is too long. Max publisher\'s name length is 255 symbols");
            }
        }

        public static void ValidateDateOfPublishing(DateTime? dateOfPublishing)
        {
            if (dateOfPublishing == null)
            {
                return;
            }

            if (dateOfPublishing > DateTime.Today)
            {
                throw new ValidationException("Date of publishing can\'t be the future one");
            }
        }

        public static void ValidateIsbn(string isbn)
        {
            IsbnValidator.ValidateIsbn(isbn);
        }

        public static void ValidateSummary(string summary)
        {
            if (IsNullOrWhiteSpace(summary))
            {
                return;
            }

            if (summary.Length > 500)
            {
                throw new ValidationException("Summary is too long. Max summary\'s length is 500 symbols");
            }
        }

        public static void ValidateAvailableCount(int? availableCount)
        {
            if (availableCount == null)
            {
                return;
            }

            if (availableCount < 0 || availableCount > 150)
            {
                throw new ValidationException(
                    "Book\'s available count is incorrect. It must be in range from 0 to 150 pieces");
            }
        }
    }
}
