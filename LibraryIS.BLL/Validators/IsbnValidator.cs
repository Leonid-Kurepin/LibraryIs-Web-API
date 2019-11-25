using System.Linq;
using LibraryIS.CommonLayer.Exceptions;
using static System.String;
using static System.Text.RegularExpressions.Regex;

namespace LibraryIS.BLL.Validators
{
    internal class IsbnValidator
    {
        internal static void ValidateIsbn(string isbn)
        {
            if (IsNullOrWhiteSpace(isbn))
            {
                //throw new ValidationException("Book must have an ISBN");
                return;
            }

            var isValidIsbn = false;

            isbn = isbn.Replace("-", "").Replace(" ", ""); // remove '-' and whitespaces

            switch (isbn.Length)
            {
                case 10:
                    isValidIsbn = IsValidIsbn10(isbn);
                    break;
                case 13:
                    isValidIsbn = IsValidIsbn13(isbn);
                    break;
            }

            if (!isValidIsbn)
            {
                throw new ValidationException("Book's ISBN isn\'t valid");
            }
        }

        private static bool IsValidIsbn10(string isbn)
        {
            bool result = false;

            // isbn10 string have 10 chars.
            // First 9 chars should be numbers and the 10th char can be a number or an 'X'
            if (IsMatch(isbn, @"^\d{9}[\d,X]{1}$"))
            {
                /*
                * result = (isbn[0] * 1 + isbn[1] * 2 + isbn[2] * 3 + isbn[3] * 4 + ... + isbn[9] * 10) mod 11 == 0
                */
                int sum = 0;

                for (int i = 0; i < 9; i++)
                {
                    sum += CharCodeToSymbolInteger(isbn[i]) * (i + 1);
                }

                sum += (isbn[9] == 'X' ? 10 : CharCodeToSymbolInteger(isbn[9])) * 10;

                result = sum % 11 == 0;
            }

            return result;
        }

        // converting char code to its symbol meaning integer
        private static int CharCodeToSymbolInteger(char c)
        {
            return c - '0';
        }

        private static bool IsValidIsbn13(string isbn)
        {
            var result = false;

            // isbn13 string have 13 chars. All of them should be numbers.
            if (IsMatch(isbn, @"^\d{13}$"))
            {
                /*
                * result = (isbn[0] * 1 + isbn[1] * 3 + isbn[2] * 1 + isbn[3] * 3 + ... + isbn[12] * 1) mod 10 == 0
                */
                int index = 0;
                int sum = isbn.Sum(c => CharCodeToSymbolInteger(c) * (IsOddNumber(index++) ? 3 : 1));

                result = sum % 10 == 0;
            }

            return result;
        }

        private static bool IsOddNumber(int number)
        {
            return number % 2 != 0;
        }
    }
}
