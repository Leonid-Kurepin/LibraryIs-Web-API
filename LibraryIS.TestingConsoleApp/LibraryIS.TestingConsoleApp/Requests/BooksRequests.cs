using System;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryIS.TestingConsoleApp.DTO;

namespace LibraryIS.TestingConsoleApp.Requests
{
    public class BooksRequests
    {
        private readonly HttpClient _client;

        public BooksRequests(HttpClient client)
        {
            _client = client;
        }

        public async Task GiveBookToMemberAsync(int bookId, int memberId, string userName)
        {
            var uri = "api/Books/" + bookId + "/out?memberId=" + memberId;
            var response = await _client.PutAsync(uri, null);

            if (response.IsSuccessStatusCode)
            {
                var retrievedBook = await response.Content.ReadAsAsync<BookDto>();

                Console.WriteLine("Book \'{0}\' was given to a member by {1}", retrievedBook.Title, userName);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        public async Task TakeBookFromMemberAsync(int bookId, int memberId, string userName)
        {
            var uri = "api/Books/" + bookId + "/in?memberId=" + memberId;
            var response = await _client.PutAsync(uri, null);

            if (response.IsSuccessStatusCode)
            {
                var retrievedBook = await response.Content.ReadAsAsync<BookDto>();

                Console.WriteLine("Book \'{0}\' was taken from a member by {1}", retrievedBook.Title, userName);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        internal async Task AddBookAsync(BookDto book)
        {
            //var book = new BookDto
            //{
            //    Title = "Fight club 2",
            //    Author = new AuthorDto()
            //    {
            //        Name = "Chuck Palahniuk"
            //    },
            //    Summary = "The first rule of Fight Club is: You do not talk about Fight Club." +
            //              "The second rule of Fight Club is: You do not talk about Fight Club.",
            //    CountAvailable = 5
            //};

            var response = await _client.PostAsJsonAsync<BookDto>("api/books", book);
 
            if (response.IsSuccessStatusCode)
            {
                var insertedBook = await response.Content.ReadAsAsync<BookDto>();

                Console.WriteLine("Book \'{0}\' was added to the library with Id: {1}", insertedBook.Title, insertedBook.Id);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }
    }
}
