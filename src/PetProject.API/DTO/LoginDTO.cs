using System.ComponentModel.DataAnnotations;

namespace PetProject.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
