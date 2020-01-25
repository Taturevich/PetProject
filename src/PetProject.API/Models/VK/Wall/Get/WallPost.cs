using Newtonsoft.Json;
using System.Collections.Generic;

namespace PetProject.Models.VK.Wall.Get
{
    public class WallPost
    {
        public int Id { get; set; }
        
        [JsonProperty("from_id  ")]
        public int PostOwnerId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        public List<Attachment> Attachments { get; set; }
    }
}
