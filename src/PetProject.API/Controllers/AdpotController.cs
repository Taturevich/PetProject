using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.DataAccess;
using PetProject.Domain;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdpotController : ControllerBase
    {
        private readonly PetContext _petContext;

        public AdpotController(PetContext petContext)
        {
            _petContext = petContext;
        }

        [HttpPost]
        [Route("{userId}/{petId}")]
        public async Task<IActionResult> AdoptRequest(int petId)
        {
            if(!int.TryParse(HttpContext.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value, out var userId))
            {
                return BadRequest();
            }

            var user = await _petContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            var pet = await _petContext.Pets.FirstOrDefaultAsync(x => x.PetId == petId);

            if(user is null || pet is null)
            {
                return NotFound();
            }

            var adoptRequest = new Adoption
            {
                PetId = petId,
                UserId = userId,
                Status = AdoptStatus.Requested
            };

            await _petContext.Adoptions.AddAsync(adoptRequest);
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{userId}/{petId}")]
        public async Task<IActionResult> AcceptPetAddopting(int userId, int petId)
        {
            var isExistsAcceptedAdoptions = await _petContext.Adoptions.AnyAsync(x => x.PetId == petId && x.Status == AdoptStatus.Accepted);
            if (isExistsAcceptedAdoptions)
            {
                return NotFound();
            }

            var existsAdopt = await _petContext.Adoptions.FirstOrDefaultAsync(x => x.PetId == petId && x.UserId == userId);
            if (existsAdopt is null)
            {
                return NotFound();
            }
            
            existsAdopt.Status = AdoptStatus.Accepted;
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{userId}/{petId}")]
        public async Task<IActionResult> DeclinePetAddopting(int userId, int petId)
        {
            var existsAdopt = await _petContext.Adoptions.FirstOrDefaultAsync(x => x.PetId == petId && x.UserId == userId);
            if (existsAdopt is null)
            {
                return NotFound();
            }
            
            existsAdopt.Status = AdoptStatus.Declined;
            await _petContext.SaveChangesAsync();
            return Ok();
        }
    }
}