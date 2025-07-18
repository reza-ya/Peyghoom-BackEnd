using Microsoft.AspNetCore.Authorization;

namespace Peyghoom_BackEnd.AAA
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }

        public ClaimRequirement(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
