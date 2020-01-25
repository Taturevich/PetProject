using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.DataAccess;
using PetProject.Domain;

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
            var pet = await _petContext.Pets.FindAsync(petId);
            var petFeatures = pet.PetFeatureAssignments.Select(pfa => pfa.PetFeature);
            return Ok(petFeatures);
        }

        // POST: api/PetFeature
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PetFeature feature)
        {
            var petFeature = _petContext.PetFeatures.Add(feature);
            return Ok(petFeature.Entity.PetFeatureId);
        }

        // PUT: api/PetFeature/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PetFeature updatedPetFeature)
        {
            try
            {
                var petFeature = await _petContext.PetFeatures.FindAsync(updatedPetFeature.PetFeatureId);
                _petContext.Entry(petFeature).CurrentValues.SetValues(updatedPetFeature);
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
