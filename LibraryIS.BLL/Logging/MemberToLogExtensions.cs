using System;
using System.Text;
using LibraryIS.DAL.Entities;

namespace LibraryIS.BLL.Logging
{
    internal static class MemberToLogExtensions
    {
        internal static StringBuilder ToLogString(this Member member)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Name: " + member.Name);
            stringBuilder.AppendLine();
            stringBuilder.Append("Passport series: " + member.PassportSeries);
            stringBuilder.AppendLine();
            stringBuilder.Append("Passport number: " + member.PassportNumber);
            stringBuilder.AppendLine();

            var dateOfBirth = (DateTime) member.DateOfBirth;
            stringBuilder.Append("Date of birth: " + dateOfBirth.Date.ToString("MM/dd/yyyy"));

            return stringBuilder;
        }
    }
}
