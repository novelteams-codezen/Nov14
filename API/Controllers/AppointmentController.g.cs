using Microsoft.AspNetCore.Mvc;
using Nov14.Models;
using Nov14.Services;
using Nov14.Entities;
using Nov14.Filter;
using Nov14.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace Nov14.Controllers
{
    /// <summary>
    /// Controller responsible for managing appointment related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting appointment information.
    /// </remarks>
    [Route("api/appointment")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        /// <summary>
        /// Initializes a new instance of the AppointmentController class with the specified context.
        /// </summary>
        /// <param name="iappointmentservice">The iappointmentservice to be used by the controller.</param>
        public AppointmentController(IAppointmentService iappointmentservice)
        {
            _appointmentService = iappointmentservice;
        }

        /// <summary>Adds a new appointment</summary>
        /// <param name="model">The appointment data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("Appointment",Entitlements.Create)]
        public IActionResult Post([FromBody] Appointment model)
        {
            var id = _appointmentService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of appointments based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of appointments</returns>
        [HttpGet]
        [UserAuthorize("Appointment",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var result = _appointmentService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <returns>The appointment data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Appointment",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _appointmentService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Appointment",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _appointmentService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <param name="updatedEntity">The appointment data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Appointment",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] Appointment updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _appointmentService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <param name="updatedEntity">The appointment data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("Appointment",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<Appointment> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _appointmentService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}