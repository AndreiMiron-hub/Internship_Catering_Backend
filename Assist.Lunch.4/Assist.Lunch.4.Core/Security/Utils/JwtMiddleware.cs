using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Assist.Lunch._4.Core.Security.Utils
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;

        public JwtMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers.Authorization
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            var userId = jwtUtils.GetUserFromToken(token);

            if (userId != null)
            {
                context.Items["User"] = await userRepository.GetByIdAsync(userId.Value);
            }

            await next(context);
        }
    }
}
