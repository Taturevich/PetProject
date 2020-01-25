using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.DataAccess;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly PetContext _petContext;

        public ImageController(PetContext petContext)
        {
            _petContext = petContext;
        }

        [HttpGet("petId")]
        public async Task<IActionResult> GetByPetId(int petId)
        {
            var images = await _petContext.Images
                .Where(p => p.PetId == petId)
                .ToListAsync();
            return Ok(images);
        }
    }
}
