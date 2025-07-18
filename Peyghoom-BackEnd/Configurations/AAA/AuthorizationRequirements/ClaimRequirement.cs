using Microsoft.AspNetCore.Authorization;

namespace Peyghoom_BackEnd.AAA
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string RequiredValue { get; }

        public ClaimRequirement(string claimType, string requiredValue = "true")
        {
            ClaimType = claimType;
            RequiredValue = requiredValue;
        }
    }
}
