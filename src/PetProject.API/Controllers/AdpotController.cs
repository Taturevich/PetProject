using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.DataAccess;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdpotController : ControllerBase
    {
        private readonly PetContext petContext;

        public AdpotController(PetContext petContext)
        {
            this.petContext = petContext;
        }

        [HttpPost]
        [Route("{userId}/{petId}")]
        public IActionResult AdoptPetrequest(int userId, int petId)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{userId}/{petId}")]
        public IActionResult AcceptPetAddopting(int userId, int petId)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{userId}/{petId}")]
        public IActionResult DeclinePetAddopting(int userId, int petId)
        {
            return Ok();
        }
    }
}