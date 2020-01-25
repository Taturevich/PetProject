using PetProject.Domain;

namespace PetProject.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public UserRole Role { get; set; }

        public int PetPoints { get; set; }

        public bool IsBlackListed { get; set; }
    }
}
