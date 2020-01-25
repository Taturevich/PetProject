using System.Collections.Generic;

namespace PetProject.Domain
{
    public class Pet
    {
        public int PetId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Volunteer { get; set; }


        public int PetStatusId { get; set; }

        public PetStatus PetStatus { get; set; }

        
        public ICollection<PetFeatureAssignment> PetFeatureAssignments { get; set; }

        public ICollection<PetTaskAssignment> PetTaskAssignments { get; set; }
    }
}
