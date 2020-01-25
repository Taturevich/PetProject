namespace PetProject.Domain
{
    public class UserFeatureAssignment
    {
        public int UserFeatureAssignmentId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int UserFeatureId { get; set; }

        public UserFeature UserFeature { get; set; }
    }
}
