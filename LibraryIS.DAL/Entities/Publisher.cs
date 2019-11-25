using System.Collections.Generic;

namespace LibraryIS.DAL.Entities
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }

        public Publisher()
        {
            Books = new HashSet<Book>();
        }
    }
}
