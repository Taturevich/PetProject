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

       /* [HttpPost]
        public IActionResult()
        {

        }*/
    }
}
