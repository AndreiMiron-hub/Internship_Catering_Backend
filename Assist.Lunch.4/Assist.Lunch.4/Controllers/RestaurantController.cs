using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using Assist.Lunch._4.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assist.Lunch._4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        IRestaurantService restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get a list of all restaurants")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<RestaurantDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get()
        {
            var response = await restaurantService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get/{restaurantId}")]
        [SwaggerOperation(Summary = "Get a restaurant")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(RestaurantDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Get(Guid restaurantId)
        {
            var response = await restaurantService.GetById(restaurantId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add new restaurant")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(RestaurantDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        public async Task<IActionResult> Add(BaseRestaurantDto baseRestaurantDto)
        {
            var response = await restaurantService.Add(baseRestaurantDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update restaurant information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(RestaurantDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Update(RestaurantDto restaurantDto)
        {
            var response = await restaurantService.Update(restaurantDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{restaurantId}")]
        [SwaggerOperation(Summary = "Delete restaurant")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Delete(Guid restaurantId)
        {
            var response = await restaurantService.Delete(restaurantId);

            return Ok(response);
        }
    }
}
