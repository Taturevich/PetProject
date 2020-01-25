using PetProject.Domain;

namespace PetProject.DTO
{
    public class PetDto
    {
        public string Name { get; set; }

        public PetType Type { get; set; }

        public int VolunteerId { get; set; }

        public int PetStatusId { get; set; }
    }
}
