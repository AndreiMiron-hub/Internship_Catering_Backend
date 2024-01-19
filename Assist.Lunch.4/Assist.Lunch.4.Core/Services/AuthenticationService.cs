using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Interfaces;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Core.Security.Utils;
using AutoMapper;

namespace Assist.Lunch._4.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService userService;
        private readonly IJwtUtils jwtUtils;
        private readonly IMapper mapper;

        public AuthenticationService(IUserService userService,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            this.userService = userService;
            this.jwtUtils = jwtUtils;
            this.mapper = mapper;
        }

        public async Task<AuthenticateResponseDto> RequestToken(RequestToken requestToken)
        {
            var user = await userService.Login(requestToken);

            var authenticateResponseDto = mapper.Map<AuthenticateResponseDto>(user);

            authenticateResponseDto.Token = jwtUtils.GenerateToken(user);

            return authenticateResponseDto;
        }
    }
}
