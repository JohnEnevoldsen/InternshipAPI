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
    public class ActivityAndGroupsOfPeopleController : ControllerBase
    {
        private readonly ActivityAndGroupsOfPeopleManager _manager;
        public ActivityAndGroupsOfPeopleController(ActivityAndGroupsOfPeopleContext context)
        {
            _manager = new ActivityAndGroupsOfPeopleManager(context);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ActivityAndGroupsOfPeople> Get()
        {
            return _manager.GetAllActivityAndGroupsOfPeople();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ActivityAndGroupsOfPeople> Post(int activityId, int groupOfPeopleId)
        {
            try
            {
                IEnumerable<ActivityAndGroupsOfPeople> newActivityAndGroupsOfPeople = _manager.addGroupToActivity(activityId, groupOfPeopleId);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + newActivityAndGroupsOfPeople;
                return Created(uri, newActivityAndGroupsOfPeople);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
