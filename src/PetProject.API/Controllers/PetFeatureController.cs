using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.DataAccess;
using PetProject.Domain;
using PetProject.DTO;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetFeatureController : ControllerBase
    {
        private ILogger<PetFeatureController> _logger;
        private PetContext _petContext;

        public PetFeatureController(ILogger<PetFeatureController> logger, PetContext petContext)
        {
            _logger = logger;
            _petContext = petContext;
        }

        // GET: api/PetFeature
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var petFeatures = await _petContext.PetFeatures.ToListAsync();
            return Ok(petFeatures);
        }

        // GET: api/PetFeature/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var petFeature = await _petContext.PetFeatures.FindAsync(id);
            return Ok(petFeature);
        }

        // GET: api/PetFeature/5
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetByPetId(int petId)
        {
            var petFeatures = _petContext.PetFeatureAssignments
                .Where(pfa => pfa.PetId == petId);

            return Ok(petFeatures);
        }

        // POST: api/PetFeature
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PetFeatureDTO petFeatureDto)
        {
            var petFeature = new PetFeature
            {
                Category = petFeatureDto.Category,
                Characteristic = petFeatureDto.Characteristic
            };

            var entry = await _petContext.PetFeatures.AddAsync(petFeature);
            await _petContext.SaveChangesAsync();

            return Ok(entry.Entity.PetFeatureId);
        }

        // PUT: api/PetFeature/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PetFeatureDTO petFeatureDto)
        {
            try
            {
                var petFeature = await _petContext.PetFeatures.FindAsync(id);
                petFeature.Category = petFeatureDto.Category;
                petFeature.Characteristic = petFeatureDto.Characteristic;
                await _petContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }

            return Ok();
        }

        [HttpPatch("{petId}")]
        public async Task<IActionResult> UpdateFeaturesOnPet(int petId, [FromBody] int[] featureIds)
        {
            try
            {
                var assignments =
                    featureIds.Select(featureId =>
                        new PetFeatureAssignment
                        {
                            PetId = petId,
                            PetFeatureId = featureId
                        });

                _petContext.Pets.Find(petId).PetFeatureAssignments.Clear();
                await _petContext.PetFeatureAssignments.AddRangeAsync(assignments);
                await _petContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
                return Problem();
            }
        }
    }
}
