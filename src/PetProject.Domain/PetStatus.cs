using System.Collections.Generic;

namespace PetProject.Domain
{
    public class PetStatus
    {
        public int PetStatusId { get; set; }

        public string Status { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}
