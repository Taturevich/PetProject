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
    public class RequestedTaskController : ControllerBase
    {
        private readonly PetContext _petContext;

        public RequestedTaskController(PetContext petContext)
        {
            _petContext = petContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _petContext.PetTaskTypeAssignments.ToListAsync());
        }

        [HttpGet]
        [Route("Pet/{id}")]
        public async Task<IActionResult> GetTaskByPet(int id)
        {
            return Ok(await _petContext.PetTaskTypeAssignments.Where(x => x.PetId == id).ToListAsync());
        }

        [HttpGet]
        [Route("Type/{id}")]
        public async Task<IActionResult> GetTaskByType(int id)
        {
            return Ok(await _petContext.PetTaskTypeAssignments.Where(x => x.TaskTypeId == id).ToListAsync());
        }
    }
}