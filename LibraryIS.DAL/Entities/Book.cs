using System;
using System.Collections.Generic;

namespace LibraryIS.DAL.Entities
{
    public class Book
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public Author Author { get; set; }
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime? DateOfPublishing { get; set; }
        public string Isbn { get; set; }
        public string Summary { get; set; }
        public int? CountAvailable { get; set; }

        public ICollection<MemberBook> MemberBooks { get; set; }

        public Book()
        {
            MemberBooks = new HashSet<MemberBook>();
        }
    }
}
