using System.Security.Claims;

namespace Peyghoom_BackEnd.Services
{
    public interface IAuthService
    {
        string GenerateToken(List<Claim> claims, DateTime dateTime, string authenticationSchema);

    }
}