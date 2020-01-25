using System;

namespace PetProject.Domain
{
    public class Task
    {
        public int TaskId { get; set; }

        /// <summary>
        /// REWORK THIS LATER
        /// </summary>
        public TaskStatus Status { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int TaskTypeId { get; set; }

        public TaskType TaskType { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PetId { get; set; }

        public Pet Pet { get; set; }
    }
}
