using System.Text.RegularExpressions;
using LibraryIS.CommonLayer.Exceptions;
using static System.String;

namespace LibraryIS.BLL.Validators
{
    internal class UserValidator
    {
        internal static void ValidateUserName(string userName, bool validateNullOrEmpty = true)
        {
            if (IsNullOrEmpty(userName))
            {
                if (validateNullOrEmpty)
                {
                    throw new ValidationException("User\'s name can\'t be empty");
                }

                return;
            }

            if (userName.Length > 200)
            {
                throw new ValidationException("User\'s name is too long. " +
                                              "Max name\'s length is 200 symbols");
            }
        }

        internal static void ValidateUserEmail(string userEmail, bool validateNullOrEmpty = true)
        {
            if (IsNullOrEmpty(userEmail))
            {
                if (validateNullOrEmpty)
                {
                    throw new ValidationException("User\'s email can\'t be empty");
                }

                return;
            }

            if (userEmail.Length > 200)
            {
                throw new ValidationException("User\'s email is too long. " +
                                              "Max email\'s length is 200 symbols");
            }

            if (!Regex.IsMatch(userEmail, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                throw new ValidationException("Incorrect email format");
            }
        }

        internal static void ValidateUserPassword(string userPassword, bool validateNullOrEmpty = true)
        {
            if (IsNullOrEmpty(userPassword))
            {
                if (validateNullOrEmpty)
                {
                    throw new ValidationException("User\'s password can\'t be empty");
                }

                return;
            }

            if (userPassword.Length > 200)
            {
                throw new ValidationException("User\'s password is too long. " +
                                              "Max password\'s length is 100 symbols");
            }

            if (!Regex.IsMatch(userPassword, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
            {
                throw new ValidationException("Incorrect password format. Password must have: \n" +
                                              "• At least one upper case english letter;\n" +
                                              "• At least one lower case english letter;\n" +
                                              "• At least one digit;\n" +
                                              "• At least one special character;\n" +
                                              "• Minimum 8 symbols length.");
            }
        }
    }
}
