using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assist.Lunch._4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get a list of all users")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<ResponseUserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get()
        {
            var response = await userService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get/{userId}")]
        [SwaggerOperation(Summary = "Get an user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseUserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var response = await userService.GetById(userId);

            return Ok(response);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update user information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseUserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            var response = await userService.Update(userDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{userId}")]
        [SwaggerOperation(Summary = "Delete user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var response = await userService.Delete(userId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("set-admin/{userId}")]
        [SwaggerOperation(Summary = "Set an account admin")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> SetAdmin(Guid userId)
        {
            var response = await userService.SetAdmin(userId);

            return Ok(response);
        }
    }
}
