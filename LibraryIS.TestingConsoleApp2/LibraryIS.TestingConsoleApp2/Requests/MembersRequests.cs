using System;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryIS.TestingConsoleApp.DTO;

namespace LibraryIS.TestingConsoleApp.Requests
{
    public class MembersRequests
    {
        private readonly HttpClient _client;

        public MembersRequests(HttpClient client)
        {
            _client = client;
        }

        public async Task AddMemberAsync(MemberDto member)
        {
            var response = await _client.PostAsJsonAsync<MemberDto>("api/members", member);

            if (response.IsSuccessStatusCode)
            {
                var insertedMember = await response.Content.ReadAsAsync<MemberDto>();

                Console.WriteLine("Member \'{0}\' was added to the library with Id: {1}", insertedMember.Name,
                    insertedMember.Id);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }
    }
}
