﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.DataAccess;
using PetProject.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    public class PetController : Controller
    {
        private readonly ILogger<PetController> _logger;
        private readonly PetContext _petContext;

        public PetController(ILogger<PetController> logger, PetContext petContext)
        {
            _logger = logger;
            _petContext = petContext;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pets = await _petContext.Pets.ToListAsync();
            return Ok(pets);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.PetId == id);
            return Ok(pet);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Pet pet)
        {
            try
            {
                await _petContext.Pets.AddAsync(pet);
                await _petContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }

            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
    }
}
