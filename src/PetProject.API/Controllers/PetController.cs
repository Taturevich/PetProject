using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.DataAccess;
using PetProject.Domain;
using PetProject.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : Controller
    {
        private readonly ILogger<PetController> _logger;
        private readonly PetContext _petContext;

        public PetController(ILogger<PetController> logger, PetContext petContext)
        {
            _logger = logger;
            _petContext = petContext;
        }

        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pets = await _petContext.Pets.ToListAsync();
            return Ok(pets);
        }

        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetByFeatureIds([FromBody]int[] featureIds)
        {
            var pets = await _petContext.Pets
                .Where(p => p.PetFeatureAssignments
                    .All(pfa => featureIds.Contains(pfa.PetFeatureId)))
                .ToListAsync();
            return Ok(pets);
        }

        // GET api/<controller>/5
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.PetId == id);
            return Ok(pet);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PetDto pet)
        {
            var petEntity = new Pet();
            try
            {
                await _petContext.Pets.AddAsync(Mapper.MapToEntity(pet, petEntity));
                await _petContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }

            return Ok(petEntity.PetId);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]PetUpdateDTO petUpdateDto)
        {
            try
            {
                var pet = await _petContext.Pets.FindAsync(petUpdateDto.PetId);

                //Pet pet;
                Mapper.MapToEntity(petUpdateDto, pet);
                //pet.Name = petUpdateDto.Name;
                //pet.Description = petUpdateDto.Description;
                //pet.Type = petUpdateDto.Type;
                //pet.VolunteerId = petUpdateDto.VolunteerId;
                //pet.PetStatusId = petUpdateDto.PetStatusId;

                await _petContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }

            return Ok();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody]int petStatusId)
        {
            try
            {
                var pet = await _petContext.Pets.FindAsync(id);
                pet.PetStatusId = petStatusId;
                await _petContext.SaveChangesAsync();
            }
            catch (Exception e) 
            {
                _logger.LogError(e, "Error");
            }

            return Ok();
        }
    }
}
