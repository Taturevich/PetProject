namespace PetProject.Domain
{
    public class PetFeatureAssignment
    {
        public int PetFeatureAssignmentId { get; set; }


        public int PetFeatureId { get; set; }

        public PetFeature PetFeature { get; set; }


        public int PetId { get; set; }

        public Pet Pet { get; set; }
    }
}
