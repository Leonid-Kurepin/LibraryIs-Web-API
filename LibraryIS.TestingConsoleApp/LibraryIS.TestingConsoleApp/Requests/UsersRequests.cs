using System;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryIS.TestingConsoleApp.DTO;

namespace LibraryIS.TestingConsoleApp.Requests
{
    class UsersRequests
    {
        private readonly HttpClient _client;

        public UsersRequests(HttpClient client)
        {
            _client = client;
        }

        public async Task AuthenticateAsync(string email, string password)
        {
            var user = new UserDto
            {
                Email = email,
                Password = password
            };

            var response = await _client.PostAsJsonAsync<UserDto>("api/users/authenticate", user);

            if (response.IsSuccessStatusCode)
            {
                var authorizedUser = await response.Content.ReadAsAsync<UserDto>();

                var accessToken = "Bearer " + authorizedUser.Token;

                _client.DefaultRequestHeaders.Add("Authorization", accessToken);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                Console.WriteLine("Authorized user: Name: {0} Email: {1}", authorizedUser.Name, authorizedUser.Email);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }
    }
}
