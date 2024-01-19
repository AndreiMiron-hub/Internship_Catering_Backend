using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;
using Assist.Lunch._4.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assist.Lunch._4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService foodService;

        public FoodController(IFoodService foodService)
        {
            this.foodService = foodService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get a list of all foods")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<ResponseFoodDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get()
        {
            var response = await foodService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get/{foodId}")]
        [SwaggerOperation(Summary = "Get a food")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseFoodDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get(Guid foodId)
        {
            var response = await foodService.GetById(foodId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a new food")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseFoodDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Food already exists")]
        public async Task<IActionResult> Add(AddFoodDto addFoodDto)
        {
            var response = await foodService.Add(addFoodDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update food information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseFoodDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Update(UpdateFoodDto updateFoodDto)
        {
            var response = await foodService.Update(updateFoodDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{foodId}")]
        [SwaggerOperation(Summary = "Delete food")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Delete(Guid foodId)
        {
            var response = await foodService.Delete(foodId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-by-restaurant/{restaurantId}")]
        [SwaggerOperation(Summary = "Get a list of foods from a specified restaurant")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<ResponseFoodDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
        {
            var response = await foodService.GetByRestaurant(restaurantId);

            return Ok(response);
        }
    }
}
