using System.Collections.Generic;

namespace PetProject.Domain
{
    public class PetFeature
    {
        public int PetFeatureId { get; set; }

        public string Category { get; set; }

        public string Characteristic { get; set; }

        public ICollection<PetFeatureAssignment> PetFeatureAssignments { get; set; }
    }
}
