using Peyghoom_BackEnd.Services.Types;
using System.Security.Claims;

namespace Peyghoom_BackEnd.Services
{
    public interface IAuthService
    {
        string GenerateToken(List<Claim> claims, DateTime dateTime);
        JwtTokens GenerateTokenRefreshToken(List<Claim> claims, DateTime accessTokenExpirationDateTime, DateTime RefreshTokenExpirationDateTime);
        ClaimsPrincipal? GetPrincipalAndValidateRefreshToken(string token);

    }
}