using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LibraryIS.TestingConsoleApp.DTO;
using LibraryIS.TestingConsoleApp.Requests;

namespace LibraryIS.TestingConsoleApp
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44330/")
        };
        static async Task Main(string[] args)
        {
            var client1 = InitializeHttpClient();
            var client2 = InitializeHttpClient();
            var client3 = InitializeHttpClient();

            var usersRequests1 = new UsersRequests(client1);
            var booksRequests1 = new BooksRequests(client1);
            var membersRequests1 = new MembersRequests(client1);

            var usersRequests2 = new UsersRequests(client2);
            var booksRequests2 = new BooksRequests(client2);
            var membersRequests2 = new MembersRequests(client2);

            var usersRequests3 = new UsersRequests(client3);
            var booksRequests3 = new BooksRequests(client3);
            var membersRequests3 = new MembersRequests(client3);

            await usersRequests1.AuthenticateAsync("admin", "111");
            var userName1 = "admin";

            await usersRequests2.AuthenticateAsync("e.mendeleeva@library.ru", "123");
            var userName2 = "user1";

            await usersRequests3.AuthenticateAsync("d.knigolyubov@library.ru", "123");
            var userName3 = "user2";

            await SendRequestsAsync(booksRequests1, 1, 1, userName1, 1);
            //await SendRequestsAsync(booksRequests2, 2, 2, userName2, 1);

            //for (int i = 0; i < 150; i++)
            //{
            //    //await booksRequests2.GiveBookToMemberAsync(2, 2, userName2);
            //    //Thread.Sleep(350);
            //    //await booksRequests2.TakeBookFromMemberAsync(2, 2, userName2);

            //    await booksRequests1.GiveBookToMemberAsync(1, 1, userName1);
            //    Thread.Sleep(1);
            //    await booksRequests1.TakeBookFromMemberAsync(1, 1, userName1);
            //}
        }
        private static async Task SendRequestsAsync(BooksRequests booksRequests, int bookId, int memberId,
            string userName, int sleepTime)
        {
            for (int i = 0; i < 150; i++)
            {
                await booksRequests.GiveBookToMemberAsync(bookId, memberId, userName);
                Thread.Sleep(sleepTime);
                await booksRequests.TakeBookFromMemberAsync(bookId, memberId, userName);
            }
        }

        private static async Task ConsoleWriteAsync(string message, int sleepTime)
        {
            Thread.Sleep(sleepTime);

            Console.WriteLine(message);
        }

        private static HttpClient InitializeHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44330/")
            };
        }

        private static async Task GetMember()
        {
            //HTTP GET
            var response = await client.GetAsync("api/members/1");

            var member = await response.Content.ReadAsAsync<MemberDto>();

            Console.WriteLine(member.Name);
        }

        private static async Task GetUser()
        {
            //HTTP GET
            var response = await client.GetAsync("api/users/1");

            var user = await response.Content.ReadAsAsync<UserDto>();

            Console.WriteLine(user.Name);
        }

        private static void AddMember()
        {
            var member = new MemberDto
            {
                Name = "Steve",
                PassportNumber = 123459,
                PassportSeries = 1234,
                DateOfBirth = new DateTime(1991, 1, 1),
                IsInBlacklist = false
            };

            var postTask = client.PostAsJsonAsync<MemberDto>("api/members", member);
            postTask.Wait();

            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<MemberDto>();
                readTask.Wait();

                var insertedMembers = readTask.Result;

                Console.WriteLine("Member {0} inserted with id: {1}", insertedMembers.Name, member.Id);
            }
            else
            {
                Console.WriteLine(result.StatusCode);
            }
        }

        private static void Authenticate()
        {
            var user = new UserDto
            {
                Email = "admin",
                Password = "111"
            };

            var postTask = client.PostAsJsonAsync<UserDto>("api/users/authenticate", user);
            postTask.Wait();

            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<UserDto>();
                readTask.Wait();

                var authorizedUser = readTask.Result;
                var accessToken = "Bearer " + authorizedUser.Token;
                client.DefaultRequestHeaders.Add("Authorization", accessToken);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                Console.WriteLine("Authorized user: Name: {0} Email: {1}", authorizedUser.Name, authorizedUser.Email);
            }
            else
            {
                Console.WriteLine(result.StatusCode);
            }
        }
    }
}
