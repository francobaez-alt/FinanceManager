using Application.Common;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorization
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
        {
            var permissions = context.User
                .FindAll(CustomClaimTypes.Permission)
                .Select(c => c.Value);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
