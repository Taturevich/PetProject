using System.Collections.Generic;

namespace PetProject.Domain
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public UserRole Role { get; set; }

        public int PetPoints { get; set; }

        public bool IsBlackListed { get; set; }

        public ICollection<UserFeatureAssignment> UserFeatureAssignments { get; set; }

        public ICollection<UserSocialNetwork> UserSocialNetworks { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
