using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class isLoginRequirement:IAuthorizationRequirement{};
public class İsLoginHandler : AuthorizationHandler<isLoginRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, isLoginRequirement requirement)
    {
        if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
};