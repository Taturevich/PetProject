using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetProject.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    public class PetController : Controller
    {
        private List<Pet> _pets = new List<Pet>
        {
            new Pet { Name = "Murka" },
            new Pet { Name = "Tuzik" },
            new Pet { Name = "Sharik" }
        };

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_pets);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _pets[id];
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            _pets.Add(value);
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
    }
}
