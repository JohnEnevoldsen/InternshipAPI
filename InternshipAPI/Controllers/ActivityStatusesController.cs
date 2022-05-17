using InternshipAPI.Manager;
using InternshipAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public ActionResult<ActivityStatus> Put([FromQuery] int id, string stringToChangeTo)
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
    }
}
