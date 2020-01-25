using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.DataAccess;
using PetProject.DTO;
using System.Threading.Tasks;

namespace PetProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTypeController : ControllerBase
    {
        private readonly PetContext _petContext;

        public TaskTypeController(PetContext petContext)
        {
            _petContext = petContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTypes()
        {
            return Ok(await _petContext.TaskTypes.AsQueryable().ToListAsync());
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetType(string name)
        {
            return Ok(await _petContext.TaskTypes.AsQueryable().FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddType([FromBody] TaskTypeDTO taskType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _petContext.TaskTypes.AddAsync(new Domain.TaskType
            {
                Name = taskType.Name,
                Description = taskType.Description,
                PetPoints = taskType.PetPoints,
                DefaultDuration = taskType.DefaultDuration
            });

            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{name}")]
        public async Task<IActionResult> UpdateType(string name, [FromBody] TaskTypeDTO taskType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dbTaskType = await _petContext.TaskTypes.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);

            if (dbTaskType is null)
            {
                return NotFound();
            }

            dbTaskType.Name = taskType.Name;
            dbTaskType.Description = taskType.Description;
            dbTaskType.PetPoints = taskType.PetPoints;
            dbTaskType.DefaultDuration = taskType.DefaultDuration;

            await _petContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var dbTaskType = await _petContext.TaskTypes.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);

            if (dbTaskType is null)
            {
                return NotFound();
            }

            _petContext.TaskTypes.Remove(dbTaskType);
            await _petContext.SaveChangesAsync();
            return Ok();
        }
    }
}
