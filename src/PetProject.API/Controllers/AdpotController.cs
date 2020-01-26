using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdpotController : ControllerBase
    {
        [HttpPost]
        [Route("{userId}/{petId}")]
        public IActionResult AdoptPetForUser(int userId, int petId)
        {
            return Ok();
        }
    }
}