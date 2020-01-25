using System;
using System.Collections.Generic;

namespace PetProject.Domain
{
    public class TaskType
    {
        public int TaskTypeId { get; set; }
        
        public string Name { get; set; }

        public int PetPoints { get; set; }

        public string Description { get; set; }

        public int DefaultDurationDays { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public ICollection<PetTaskTypeAssignment> PetTaskTypeAssignments { get; set; }
    }
}
