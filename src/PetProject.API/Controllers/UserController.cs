using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetProject.DataAccess;
using PetProject.Domain;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly PetContext _petContext;

        public UserController(PetContext petContext)
        {
            _petContext = petContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var pets = _petContext.Pets.ToList();
            return Ok(pets);
        }
    }
}
