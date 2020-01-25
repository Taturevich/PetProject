namespace PetProject.Domain
{
    public class UserSocialNetwork
    {
        public int UserSocialNetworkId { get; set; }

        public string NetworkType { get; set; }

        public string Link { set; get; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
