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
    public class UserFeatureController : ControllerBase
    {
        private ILogger<PetFeatureController> _logger;
        private PetContext _petContext;

        public UserFeatureController(ILogger<PetFeatureController> logger, PetContext petContext)
        {
            _logger = logger;
            _petContext = petContext;
        }

        // GET: api/PetFeature
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var petFeatures = await _petContext.UserFeatures.ToListAsync();
            return Ok(petFeatures);
        }

        // GET: api/PetFeature/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var petFeature = await _petContext.UserFeatures.FindAsync(id);
            return Ok(petFeature);
        }

        // POST: api/PetFeature
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] PetFeatureDTO featureDTO)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var oldFeature = await _petContext
                .UserFeatures
                .FirstOrDefaultAsync(x => x.Category == featureDTO.Category && x.Characteristic == featureDTO.Characteristic);    
            if (oldFeature != null)
            {
                BadRequest();
            }

            await _petContext.UserFeatures.AddAsync(Mapper.MapToEntity(featureDTO, new UserFeature()));
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/PetFeature/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PetFeatureDTO featureDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var oldFeature = await _petContext.UserFeatures.FindAsync(id);
            if (oldFeature is null)
            {
                NotFound();
            }

            Mapper.MapToEntity(featureDto, oldFeature);
            await _petContext.SaveChangesAsync();
            return Ok();
        }
    }
}
