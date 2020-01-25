using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    public class AnimalController : Controller
    {
        private List<string> _animals = new List<string>
        {
            "Murka",
            "Tuzik",
            "Sharik"
        };

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_animals);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _animals[id];
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            _animals.Add(value);
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
    }
}
