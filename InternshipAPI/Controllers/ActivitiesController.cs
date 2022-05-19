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
    public class ActivitiesController : ControllerBase
    {
        private readonly ActivityManager _manager;
        public ActivitiesController(ActivityContext context)
        {
            _manager = new ActivityManager(context);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Activity> Get()
        {
            return _manager.GetAllActivities();
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Activity Get(int id)
        {
            return _manager.GetActivityById(id);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Activity> Post([FromBody] Activity value)
        {
            try
            {
                Activity newActivity = _manager.AddActivity(value);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + newActivity.Id;
                return Created(uri, newActivity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("OnePersonAndTheirActivities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Activity> GetAllActitiviesWithThatPerson(int personId)
        {
            return _manager.GetAllActivitiesWithPersonId(personId);
        }
    }
}
