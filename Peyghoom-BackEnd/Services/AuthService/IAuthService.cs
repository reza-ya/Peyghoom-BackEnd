using Peyghoom_BackEnd.Services.Types;
using System.Security.Claims;

namespace Peyghoom_BackEnd.Services
{
    public interface IAuthService
    {
        string GenerateToken(List<Claim> claims, string authenticationSchema);

        Task GetVerifyJwtAndGetClaimsAsync(string token);
    }
}