using System.ComponentModel.DataAnnotations;

namespace PetProject.DTO
{
    public class PetFeatureDTO
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Characteristic { get; set; }
    }
}
