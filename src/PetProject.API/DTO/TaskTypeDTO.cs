using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PetProject.DTO
{
    public class TaskTypeDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int PetPoints { get; set; }
        
        public DateTime DefaultDuration { get; set; }
    }
}
