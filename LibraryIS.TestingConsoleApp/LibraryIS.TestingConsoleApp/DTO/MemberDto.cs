using System;

namespace LibraryIS.TestingConsoleApp.DTO
{
    public class MemberDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? PassportNumber { get; set; }
        public int? PassportSeries { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CountOfBooks { get; set; }
        public bool? IsInBlacklist { get; set; }
    }
}
