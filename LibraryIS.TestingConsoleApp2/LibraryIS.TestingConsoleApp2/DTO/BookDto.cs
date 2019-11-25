using System;
using LibraryIS.TestingConsoleApp.DTO;

namespace LibraryIS.TestingConsoleApp.DTO
{
    public class BookDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public AuthorDto Author { get; set; }
        public PublisherDto Publisher { get; set; }
        public DateTime? DateOfPublishing { get; set; }
        public string Isbn { get; set; }
        public string Summary { get; set; }
        public int? CountAvailable { get; set; }
    }
}
