
using Microsoft.AspNetCore.Authorization;

namespace Peyghoom_BackEnd.AAA
{
    public class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            var hasClaim = context.User.HasClaim(c => c.Type == requirement.ClaimType && c.Value == requirement.RequiredValue);

                context.Succeed(requirement);
            if (hasClaim)
            {
            }

            return Task.CompletedTask;
        }
    }
}
