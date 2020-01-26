namespace PetProject.Domain
{
    public class Adoption
    {
        public int AdoptionId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PetId { get; set; }

        public Pet Pet { get; set; }

        public AdoptStatus Status { get; set; }
    }
}
