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
        [Route("")]
        public async Task<IActionResult> GetUsers()
        {
            var pets = await _petContext.Pets.ToListAsync();
            return Ok(pets);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _petContext.Users.FirstOrDefaultAsync(x => x.Phone == userDTO.Phone);
            if (user != null)
            {
                return BadRequest();
            }

            await _petContext.Users.AddAsync(Mapper.MapToEntity(userDTO, new User()));
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _petContext.Users.FindAsync(userId);
            if (user is null)
            {
                return BadRequest();
            }

            Mapper.MapToEntity(userDTO, user);
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        // GET: api/PetFeature/5
        [HttpGet]
        [Route("{userId}/Features")]
        public async Task<IActionResult> GetFeaturesByUserId(int userId)
        {
            var assignedFeatures = await _petContext
                .UserFeatureAssignments
                .Where(x => x.UserId == userId)
                .Select(x => x.UserFeatureId)
                .ToListAsync();
            if (assignedFeatures is null || !assignedFeatures.Any())
            {
                return Ok();
            }

            var features = await _petContext
                .UserFeatures
                .Where(x => assignedFeatures.Contains(x.UserFeatureId))
                .ToListAsync();
            return Ok(features);
        }

        [HttpPut]
        [Route("{userId}/Features")]
        public async Task<IActionResult> UpdateFeaturesBatch(int userId, [FromBody] int[] featureIds)
        {
            if (featureIds is null || !featureIds.Any())
            {
                return BadRequest();
            }

            var oldFeatures = await _petContext
                .UserFeatureAssignments
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _petContext.RemoveRange(oldFeatures);
            foreach (var featureId in featureIds)
            {
                await _petContext.UserFeatureAssignments.AddAsync(new UserFeatureAssignment
                {
                    UserId = userId,
                    UserFeatureId = featureId
                });
            }
            await _petContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{id}/BlackList")]
        public async Task<IActionResult> BlackList(int id)
        {
            var user = await _petContext.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            user.IsBlackListed = true;
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/BlackList")]
        public async Task<IActionResult> RemoveBlackList(int id)
        {
            var user = await _petContext.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            user.IsBlackListed = false;
            await _petContext.SaveChangesAsync();
            return Ok();
        }
    }
}
