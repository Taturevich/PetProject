using System;

namespace PetProject.Domain
{
    public class TaskType
    {
        public int TaskTypeId { get; set; }
        
        public string Name { get; set; }

        public int PetPoints { get; set; }

        public string Description { get; set; }

        public DateTime DefaultDuration { get; set; }
    }
}
