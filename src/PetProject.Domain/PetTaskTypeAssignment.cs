namespace PetProject.Domain
{
    public class PetTaskTypeAssignment
    {
        public int PetTaskTypeAssignmentId { get; set; }

        public int PetId { get; set; }

        public Pet Pet { get; set; }

        public int TaskTypeId { get; set; }

        public TaskType TaskType { get; set; }
    }
}
