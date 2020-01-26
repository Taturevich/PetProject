namespace PetProject.Domain
{
    public class Adopt
    {
        public int AdoptId { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public AdoptStatus Status { get; set; }
    }
}
