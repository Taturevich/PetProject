using Newtonsoft.Json;

namespace PetProject.Models.VK.Attachments
{
    public class Photo
    {
        public int Id { get; set; }

        [JsonProperty("album_id")]
        public string AlbumId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
