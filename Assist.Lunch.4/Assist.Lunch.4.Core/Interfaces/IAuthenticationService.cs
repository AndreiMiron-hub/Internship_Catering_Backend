using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Models;

namespace Assist.Lunch._4.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponseDto> RequestToken(RequestToken requestToken);
    }
}
