using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YemekTarifleri.Authorizon;
public class isAdminRequirement:IAuthorizationRequirement{};
public class isAdminHandler : AuthorizationHandler<isAdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, isAdminRequirement requirement)
    {
        var Roles = context.User.FindFirstValue(ClaimTypes.Role);
        if (Roles == "admin")
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}