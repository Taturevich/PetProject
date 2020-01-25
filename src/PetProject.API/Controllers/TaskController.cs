using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.DataAccess;
using PetProject.DTO;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly PetContext _petContext;

        public TaskController(PetContext petContext)
        {
            _petContext = petContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _petContext.Tasks.AsQueryable().ToListAsync());
        }

        [HttpPost]
        [Route("Assign")]
        public async Task<IActionResult> AssingToTask()
        {
            return Ok(await _petContext.Tasks.AsQueryable().ToListAsync());
        }
    }
}
