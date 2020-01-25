namespace PetProject.Services.VK
{
    public class VKServiceBase 
    {
        protected const string AuthToken = "0a2eb4fd0a2eb4fd0a2eb4fd6b0a41f9c600a2e0a2eb4fd54056bf23d8b06fe8fcca071";
        protected const string VKAPiVersion = "5.103";
        protected const string VKApiRequestTemplate = "https://api.vk.com/method/";
        protected const string VKCommonPrereqisites = "&access_token={AuthToken}&v={VKAPiVersion}";

        protected string GetURI(string methodData) => $"{VKApiRequestTemplate}{methodData}{VKCommonPrereqisites}";
    }
}
