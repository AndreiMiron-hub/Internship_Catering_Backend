using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;
using Assist.Lunch._4.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assist.Lunch._4.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DestinationController : ControllerBase
    {
        readonly IDestinationService destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            this.destinationService = destinationService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get a list of all destinations")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(IEnumerable<ResponseDestinationDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        public async Task<IActionResult> Get()
        {
            var response = await destinationService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get/{destinationId}")]
        [SwaggerOperation(Summary = "Get a destination by Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDestinationDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get(Guid destinationId)
        {
            var response = await destinationService.GetById(destinationId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a new delivery destination")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDestinationDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Entity already exists")]
        public async Task<IActionResult> Add(BaseDestinationDto baseDestinationDto)
        {
            var response = await destinationService.Add(baseDestinationDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update destination information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDestinationDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Update(DestinationDto destinationDto)
        {
            var response = await destinationService.Update(destinationDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{destinationId}")]
        [SwaggerOperation(Summary = "Delete destination")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Delete(Guid destinationId)
        {
            var response = await destinationService.Delete(destinationId);

            return Ok(response);
        }
    }
}
