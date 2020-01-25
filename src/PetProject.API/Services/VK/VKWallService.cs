using PetProject.DataAccess;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetProject.Services.VK
{
    public class VKWallService : VKServiceBase, IVKWallService
    {
        private readonly HttpClient _client;
        private readonly PetContext _petContext;

        public VKWallService(IHttpClientFactory clientFactory, PetContext petContext)
        {
            _client = clientFactory.CreateClient();
            _petContext = petContext;
        }

        //ToDo VK DBContext
        public async Task AddNewGroup(string domain)
        {
            //ToDO insert into the DB group for the feature event based post inserting

            await ParseGroupAndInsert(domain);
        }

        public async Task ParseGroup(string domain)
        {
            await ParseGroupAndInsert(domain);
        }

        private async Task ParseGroupAndInsert(string domain)
        {
            var url = GetURI($"wall.get?domain={domain}");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
          //  var data = await JsonSerializer.DeserializeAsync
             //   <IEnumerable<GitHubBranch>>(responseStream);         
        }
    }
}
