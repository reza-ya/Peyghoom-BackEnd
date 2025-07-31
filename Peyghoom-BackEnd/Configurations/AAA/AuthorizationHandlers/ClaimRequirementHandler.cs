
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Peyghoom_BackEnd.AAA
{
    public class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            // TODO: authorize by policy for Hub
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var hasClaim = context.User.HasClaim(c => c.Type == requirement.ClaimType);

            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }



        private bool IsUserAllowedToDoThis(string hubMethodName, string currentUsername)
        {
            return !(currentUsername.Equals("asdf42@microsoft.com") &&
                hubMethodName.Equals("banUser", StringComparison.OrdinalIgnoreCase));
        }
    }
}
