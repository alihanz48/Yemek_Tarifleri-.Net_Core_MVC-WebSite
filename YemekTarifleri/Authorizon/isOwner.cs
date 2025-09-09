using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using YemekTarifleri.Data.Abstract;

public class isOwnerRequirement : IAuthorizationRequirement { }

public class isOwnerHandler : AuthorizationHandler<isOwnerRequirement>
{
    private readonly IFoodRepository _foodRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public isOwnerHandler(IFoodRepository foodRepository, IHttpContextAccessor httpContextAccessor)
    {
        _foodRepository = foodRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, isOwnerRequirement requirement)
    {
        var routeValue = _httpContextAccessor.HttpContext!.Request.RouteValues;

        if (routeValue.TryGetValue("url", out var foodUrl))
        {
            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId != null)
            {
                var food = _foodRepository.Foods.FirstOrDefault(f => f.url == foodUrl && f.UserID == int.Parse(userId));

                if (food != null)
                {
                    context.Succeed(requirement);
                }
            }

        }
        return Task.CompletedTask;
    }
}
