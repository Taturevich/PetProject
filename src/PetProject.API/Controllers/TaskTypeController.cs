using Microsoft.AspNetCore.Mvc;
using PetProject.DataAccess;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTypeController : ControllerBase
    {
        private readonly PetContext petContext;

        public TaskTypeController(PetContext petContext)
        {
            this.petContext = petContext;
        }

        [HttpPost]
        public IActionResult AddType()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateType()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}
