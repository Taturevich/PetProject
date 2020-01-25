using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using PetProject.Services.VK;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AddNewWall([FromBody]VKWallRequest group)
        {
            await vkService.AddNewGroup(group.Domain);
            return Ok();
        }

        [HttpPost]
        [Route("Wall/Parse")]
        public async Task<IActionResult> ParseWall([FromBody]VKWallRequest group)
        {
            await vkService.AddNewGroup(group.Domain);
            return Ok();
        }
    }
}