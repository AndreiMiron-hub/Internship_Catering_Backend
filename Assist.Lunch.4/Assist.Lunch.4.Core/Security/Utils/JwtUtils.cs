using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Assist.Lunch._4.Core.Security.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly TokenSettings tokenSetting;

        public JwtUtils(IOptions<TokenSettings> tokenSettings)
        {
            this.tokenSetting = tokenSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSetting.TokenKey);
            var userRole = user.IsAdmin ? "Admin" : "User";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {   new Claim("id", user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("role", userRole)
                }),
                Expires = DateTime.UtcNow.AddHours(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Guid? GetUserFromToken(string token)
        {
            if (token is null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            return Guid.Parse(jwtSecurityToken.Claims.First(x => x.Type == "id").Value);
        }

        public void CheckForOwnership(IHttpContextAccessor context, Guid userId)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                if(!userId.Equals(Guid.Parse(identity.FindFirst("id").Value)))
                {
                    throw new InvalidCredentialsException(UserResources.Unauthorized);
                }
            }
        }
    }
}
