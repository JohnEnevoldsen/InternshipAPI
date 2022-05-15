using InternshipAPI.Manager;
using InternshipAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace InternshipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly PersonManager _manager;
        public PersonsController(PersonContext context)
        {
            _manager = new PersonManager(context);
        }
        // GET: api/<PersonsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Person> Get()
        {
            return _manager.GetAllPersons();
        }
        [HttpGet("OnePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> GetOnePerson([FromQuery] string mail, string passWord)
        {
            IEnumerable<Person> person = _manager.GetOnePerson(mail, passWord);
            string jsonString = JsonSerializer.Serialize(person);
            if (jsonString == "[]") {
                return NotFound("Der findes ikke en bruger med email: " + mail + " og med det kodeord");
            }
            return Ok(person);
        }
        [HttpPut("PutPath")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Put([FromQuery] string email, string oldPassWord, string newPassWord)
        {
            try
            {
                Person updatedPerson = _manager.Update(email, oldPassWord, newPassWord);
                if (updatedPerson == null) return NotFound("Der findes ikke en bruger med denne email: " + email);
                return Ok(updatedPerson);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
