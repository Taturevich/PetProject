namespace PetProject.DTO
{
    public class PetUpdateDTO : PetDto
    {
        public int PetId { get; set; }

        public string Description { get; set; }
    }
}
