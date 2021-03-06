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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Person Get(int id)
        {
            return _manager.GetPersonById(id);
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
        public ActionResult<Person> PutPassword([FromQuery] string email, string oldPassword, string newPassword)
        {
            try
            {
                Person updatedPerson = _manager.UpdatePassword(email, oldPassword, newPassword);
                if (updatedPerson == null) return NotFound("Der findes ikke en bruger med denne email: " + email);
                return Ok(updatedPerson);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person> Post([FromBody] Person value)
        {
            try
            {
                Person newPerson = _manager.AddPerson(value);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + newPerson.Id;
                return Created(uri, newPerson);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Delete(int id)
        {
            Person deletedPerson = _manager.DeletePerson(id);
            if (deletedPerson == null) return NotFound("Der findes ikke en person med denne id: " + id);
            return Ok(deletedPerson);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Put(int id, [FromBody] Person value)
        {
            try
            {
                Person updatedPerson = _manager.UpdatePerson(id, value);
                if (updatedPerson == null) return NotFound("No such person, id: " + id);
                return Ok(updatedPerson);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
