using InternshipAPI.Manager;
using InternshipAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InternshipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly GroupManager _manager;
        public GroupsController(GroupContext context)
        {
            _manager = new GroupManager(context);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Group> Get()
        {
            return _manager.GetAllGroups();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Group> Post([FromBody] Group value)
        {
            try
            {
                Group newGroup = _manager.AddGroup(value);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + newGroup.Id;
                return Created(uri, newGroup);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Group> Delete(int id)
        {
            Group deletedGroup = _manager.DeleteGroup(id);
            if (deletedGroup == null) return NotFound("Der findes ikke en gruppe med denne id: " + id);
            return Ok(deletedGroup);
        }
    }
}
