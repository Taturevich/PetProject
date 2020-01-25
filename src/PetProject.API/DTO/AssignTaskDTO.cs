using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PetProject.DTO
{
    public class AssignTaskDTO
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string TaskId { get; set; }

        [Required]
        public string PetId { get; set; }
    }
}
