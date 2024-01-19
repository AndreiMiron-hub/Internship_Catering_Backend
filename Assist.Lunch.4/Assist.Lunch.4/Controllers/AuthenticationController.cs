using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Interfaces;
using Assist.Lunch._4.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assist.Lunch._4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService,
            IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Authenticate user and generate token")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(AuthenticateResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        public async Task<IActionResult> Authenticate(RequestToken requestToken)
        {
            var response = await authenticationService.RequestToken(requestToken);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseUserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "User already exists")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var response = await userService.Register(registerUserDto);

            return Ok(response);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("update-password")]
        [SwaggerOperation(Summary = "Update user password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Entity not found")]
        public async Task<IActionResult> UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto)
        {
            var response = await userService.UpdatePassword(updateUserPasswordDto);

            return Ok(response);
        }
    }
}
