using Assist.Lunch._4.Domain.Entitites;
using Microsoft.AspNetCore.Http;

namespace Assist.Lunch._4.Core.Security.Utils
{
    public interface IJwtUtils
    {
        string GenerateToken(User user);
        Guid? GetUserFromToken(string token);
        void CheckForOwnership(IHttpContextAccessor context, Guid userId);
    }
}
