using PetProject.Domain;
using System.ComponentModel.DataAnnotations;

namespace PetProject.DTO
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
