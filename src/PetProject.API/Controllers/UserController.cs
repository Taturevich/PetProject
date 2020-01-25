using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.DataAccess;
using PetProject.Domain;
using PetProject.DTO;

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
        public async Task<IActionResult> Get()
        {
            var pets = await _petContext.Pets.ToListAsync();
            return Ok(pets);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDTO user)
        {
            await _petContext.Users.AddAsync(Mapper.MapToEntity(user, new User()));
            await _petContext.SaveChangesAsync();
            return Ok();
        }
    }
}
