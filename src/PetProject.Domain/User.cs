namespace PetProject.Domain
{
    public class User
    {
        public int UserId { get; set; }

        public int Name { get; set; }

        public int LastName { get; set; }

        public string Phone { get; set; }

        public int Role { get; set; }

        public int PetPoints { get; set; }

        public bool IsBlackListed { get; set; }
    }
}
