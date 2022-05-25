using InternshipAPI.Manager;
using InternshipAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternshipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityStatusesController : ControllerBase
    {
        private readonly ActivityStatusManager _manager;
        public ActivityStatusesController(ActivityStatusContext context)
        {
            _manager = new ActivityStatusManager(context);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ActivityStatus> Get()
        {
            return _manager.GetAllActivityStatuses();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ActivityStatus> Post([FromQuery] int activityId, int personId)
        {
            try
            {
                ActivityStatus newActivityStatus = _manager.AddActivityStatus(activityId, personId);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + newActivityStatus.Id;
                return Created(uri, newActivityStatus);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ActivityStatus> Put(int id, [FromQuery] string stringToChangeTo)
        {
            try
            {
                ActivityStatus updatedActivityStatus = _manager.Update(id, stringToChangeTo);
                if (updatedActivityStatus == null) return NotFound("Her er statusToChangeTo: " + stringToChangeTo + " Her er Id: " + id);
                return Ok(updatedActivityStatus);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Activities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ActivityStatus> GetByActivityId(int activityId)
        {
            IEnumerable<ActivityStatus> activityStatuses = _manager.GetAllByActivity(activityId);
            if (activityStatuses.Any())
            {
                return Ok(activityStatuses);
            }
            return NotFound("Der er ikke nogle aktiviteter med id: " + activityId);
        }
    }
}
