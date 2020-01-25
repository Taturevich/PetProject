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
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var pets = await _petContext.Pets.ToListAsync();
            return Ok(pets);
        }

        [HttpPost]
        [Route("PostUsers")]
        public async Task<IActionResult> PostUser(UserDTO user)
        {
            await _petContext.Users.AddAsync(Mapper.MapToEntity(user, new User()));
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

        [HttpPut("{id}/Features")]
        public async Task<IActionResult> UpdateBatch(int id, [FromBody] int[] featureIds)
        {
            if(featureIds is null || !featureIds.Any())
            {
                return BadRequest();
            }

            var oldFeatures = await _petContext
                .UserFeatureAssignments
                .Where(x => x.UserId == id)
                .ToListAsync();

            _petContext.RemoveRange(oldFeatures);
            foreach (var featureId in featureIds)
            {
                await _petContext.UserFeatureAssignments.AddAsync(new UserFeatureAssignment
                {
                    UserId = id,
                    UserFeatureId = featureId
                });
            }
            await _petContext.SaveChangesAsync();

            return Ok();
        }
    }
}
