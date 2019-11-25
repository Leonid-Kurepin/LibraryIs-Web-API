using System;
using LibraryIS.CommonLayer.Exceptions;
using static System.String;

namespace LibraryIS.BLL.Validators
{
    internal class MemberValidator
    {
        internal static void ValidateMemberName(string memberName, bool validateNullOrEmpty = true)
        {
            if (IsNullOrEmpty(memberName))
            {
                if (validateNullOrEmpty)
                {
                    throw new ValidationException("Member\'s name can\'t be empty");
                }

                return;
            }

            if (memberName.Length > 200)
            {
                throw new ValidationException("Member\'s name is too long. Max name\'s length is 200 symbols");
            }
        }

        internal static void ValidateMemberPassportNumber(int? passportNumber, bool validateNull = true)
        {
            if (passportNumber == null)
            {
                if (validateNull)
                {
                    throw new ValidationException("Member\'s passport number can\'t be empty");
                }

                return;
            }

            if (passportNumber < 100000 || passportNumber > 999999)
            {
                throw new ValidationException("Member\'s passport number is incorrect");
            }
        }

        internal static void ValidateMemberPassportSeries(int? passportSeries, bool validateNull = true)
        {
            if (passportSeries == null)
            {
                if (validateNull)
                {
                    throw new ValidationException("Member\'s passport series can\'t be empty");
                }

                return;
            }

            if (passportSeries < 1000 || passportSeries > 9999)
            {
                throw new ValidationException("Member\'s passport series is incorrect");
            }
        }

        internal static void ValidateMemberBirthDate(DateTime? birthDate, bool validateNull = true)
        {
            if (birthDate == null)
            {
                if (validateNull)
                {
                    throw new ValidationException("Member\'s birthdate can\'t be empty");
                }

                return;
            }

            if (birthDate < new DateTime(1900, 1, 1) || birthDate > DateTime.Today)
            {
                throw new ValidationException("Member\'s birthdate is incorrect");
            }

            var memberAge = MemberAge((DateTime) birthDate);

            if (memberAge < 14)
            {
                throw new ValidationException("Member is too young. He must be at least 14 y.o.");
            }
        }

        private static int MemberAge(DateTime birthDate)
        {
            var currentDate = DateTime.Now;

            return (currentDate.Year - birthDate.Year - 1) +
                   (((currentDate.Month > birthDate.Month) ||
                     ((currentDate.Month == birthDate.Month) && (currentDate.Day >= birthDate.Day)))
                       ? 1
                       : 0);
        }
    }
}
