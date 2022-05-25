using InternshipAPI.Manager;
using InternshipAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InternshipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly StatusManager _manager;

        public StatusesController(StatusContext context)
        {
            _manager = new StatusManager(context);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Status> Get()
        {
            return _manager.GetAllStatuses();
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Status Get(int id)
        {
            return _manager.GetStatusById(id);
        }
    }
}
