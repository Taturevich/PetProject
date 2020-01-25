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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddType([FromBody] TaskTypeDTO taskType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _petContext.TaskTypes.AddAsync(Mapper.MapToEntity(taskType, new Domain.TaskType()));

            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetType(string name)
        {
            return Ok(await _petContext.TaskTypes.AsQueryable().FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPut]
        [Route("{taskId}")]
        public async Task<IActionResult> UpdateType(int taskId, [FromBody] TaskTypeDTO taskType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = await _petContext.TaskTypes.FindAsync(taskId);

            if (entity is null)
            {
                return NotFound();
            }

            Mapper.MapToEntity(taskType, entity);
            await _petContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{name}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            var dbTaskType = await _petContext.TaskTypes.FindAsync(taskId);

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
