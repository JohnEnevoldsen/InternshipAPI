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
    public class GroupsOfPeopleController : ControllerBase
    {
        private readonly GroupOfPeopleManager _manager;
        public GroupsOfPeopleController(GroupOfPeopleContext context)
        {
            _manager = new GroupOfPeopleManager(context);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<GroupOfPeople> Get()
        {
            return _manager.GetAllGroupsOfPeople();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<GroupOfPeople> Post(int groupId, int personId)
        {
            try
            {
                GroupOfPeople newGroupOfPeople = _manager.AddPersonToGroup(groupId, personId);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + newGroupOfPeople.Id;
                return Created(uri, newGroupOfPeople);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
