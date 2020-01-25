using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.DataAccess;
using PetProject.Domain;
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
            return Ok(await _petContext.Tasks.ToListAsync());
        }

        [HttpGet]
        [Route("Pet/{id}")]
        public async Task<IActionResult> GetTaskByPet(int id)
        {
            return Ok(await _petContext.Tasks.Where(x => x.PetId == id).ToListAsync());
        }

        [HttpGet]
        [Route("Type/{id}")]
        public async Task<IActionResult> GetTaskByType(int id)
        {
            return Ok(await _petContext.Tasks.Where(x => x.TaskTypeId == id).ToListAsync());
        }

        [HttpGet]
        [Route("User/{id}")]
        public async Task<IActionResult> GetTaskByUser(int id)
        {
            return Ok(await _petContext.Tasks.Where(x => x.UserId == id).ToListAsync());
        }

        [HttpPost]
        [Route("Request/{typeId}/{petId}")]
        public async Task<IActionResult> RequestTask(int typeId, int petId)
        {
            var existsAssigment = await _petContext
                .PetTaskTypeAssignments
                .FirstOrDefaultAsync(x => x.PetId == petId && x.TaskTypeId == typeId );

            if (existsAssigment != null)
            {
                return BadRequest();
            }

            await _petContext.PetTaskTypeAssignments.AddAsync(new PetTaskTypeAssignment { TaskTypeId = typeId, PetId = petId });
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("Assign/{typeId}/{petId}/{userId}")]
        public async Task<IActionResult> AssingToTask(int typeId, int petId, int userId)
        {
            var existsAssigment = await _petContext
                .PetTaskTypeAssignments
                .FirstOrDefaultAsync(x => x.PetId == petId && x.TaskTypeId == typeId);

            if (existsAssigment is null)
            {
                return NotFound();
            }

            var existsTask = await _petContext
                .Tasks
                .FirstOrDefaultAsync(x =>
                x.PetId == petId &&
                x.TaskTypeId == typeId &&
                x.UserId == userId &&
                x.Status == Domain.TaskStatus.InProgress);

            if (existsTask != null)
            {
                return BadRequest();
            }

            var task = new Domain.Task
            {
                PetId = petId,
                TaskTypeId = typeId,
                UserId = userId,
                StartDate = DateTime.UtcNow,
                Status = Domain.TaskStatus.InProgress
            };

            await _petContext.Tasks.AddAsync(task);
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("Stop/{taskId}")]
        public async Task<IActionResult> StopTask(int taskId)
        {
            var task = await _petContext.Tasks.FindAsync(taskId);
            if (task is null)
            {
                return NotFound();
            }

            task.Status = Domain.TaskStatus.Failed;
            task.EndDate = DateTime.UtcNow;
            await _petContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("Complete/{taskId}")]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var task = await _petContext.Tasks.FindAsync(taskId);
            if (task is null)
            {
                return NotFound();
            }

            task.Status = Domain.TaskStatus.Completed;
            task.EndDate = DateTime.UtcNow;
            await _petContext.SaveChangesAsync();
            return Ok();
        }
    }
}
