using System.Net.Http;

namespace PetProject.Services.VK
{
    public class VKWallService : VKServiceBase, IVKWallService
    {
        private readonly IHttpClientFactory _clientFactory;

        public VKWallService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        //ToDo VK DBContext
        public void AddNewGroup(string domain)
        {
            //ToDO insert into the DB group for the feature event based post inserting

            ParseGroupAndInsert(domain);
        }

        public void ParseGroup(string domain)
        {
            ParseGroupAndInsert(domain);
        }

        private void ParseGroupAndInsert(string domain)
        {
            var url = GetURI($"wall.get?domain={domain}");
        }
    }
}
