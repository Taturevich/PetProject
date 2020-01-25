using System.Collections.Generic;

namespace PetProject.Domain
{
    public class UserFeature
    {
        public int UserFeatureId { get; set; }

        public string Category { get; set; }

        public int Characteristic { get; set; }

        public ICollection<UserFeatureAssignment> UserFeatureAssignments { get; set; }
    }
}
