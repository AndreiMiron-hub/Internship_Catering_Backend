using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assist.Lunch._4.Core.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

            if (allowAnonymous)
            {
                return;
            }

            var user = (User)context.HttpContext.Items["User"];

            if (user is null)
            {
                context.Result = new JsonResult(new { message = UserResources.Unauthorized }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
