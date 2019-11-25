using System;
using System.Collections.Generic;

namespace LibraryIS.DAL.Entities
{
    public class Member
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? PassportNumber { get; set; }
        public int? PassportSeries { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsInBlacklist { get; set; }

        public ICollection<MemberBook> MemberBooks { get; set; }

        public Member()
        {
            MemberBooks = new HashSet<MemberBook>();
        }
    }
}
