using System;

namespace PetProject.Domain
{
    public class Task
    {
        public int TaskId { get; set; }

        public int UserID { get; set; }

        public int PetId { get; set; }

        /// <summary>
        /// REWORK THIS LATER
        /// </summary>
        public TaskStatus Status { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int TaskTypeId { get; set; }

        public TaskType TaskType { get; set; }
    }
}
