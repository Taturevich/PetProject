using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.DataAccess;

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

        // GET: api/PetChar
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var petFeatures = await _petContext.PetFeatures.ToListAsync();
            return Ok(petFeatures);
        }

        // GET: api/PetChar/5
        [HttpGet("{id}", Name = "Get")]
        public string GetById(int id)
        {
            return "value";
        }

        // POST: api/PetFeature
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PetFeature/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
