using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services.VK;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VkController : ControllerBase
    {
        private readonly IVKWallService vkService;

        public VkController(IVKWallService vkService)
        {
            this.vkService = vkService;
        }

        [HttpPost]       
        [Route("Wall")]
        public IActionResult AddNewWall([FromBody]VKWallRequest group)
        {
            return Ok();
        }

        [HttpPost]
        [Route("Wall/Parse")]
        public IActionResult ParseWall([FromBody]VKWallRequest group)
        {
            return Ok();
        }
    }
}