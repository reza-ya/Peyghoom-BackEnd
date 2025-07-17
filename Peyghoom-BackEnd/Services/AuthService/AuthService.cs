
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Peyghoom_BackEnd.Options;
using Peyghoom_BackEnd.Services.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Peyghoom_BackEnd.Services
{
    public class AuthService : IAuthService
    {
        private VerifyAuthSchemaOptions _verifyAuthSchemaOptions;
        private MainAuthSchemaOptions _mainAuthSchemaOptions;
        public AuthService(IOptionsSnapshot<VerifyAuthSchemaOptions> verifyOptionsSnapshot, IOptionsSnapshot<MainAuthSchemaOptions> mainOptionsSnapshot)
        {
            _verifyAuthSchemaOptions = verifyOptionsSnapshot.Value;
            _mainAuthSchemaOptions = mainOptionsSnapshot.Value;

        }

        public string GenerateToken(List<Claim> claims, string authenticationSchema)
        {
            string? secretKey = null;
            string? issuer = null;

            if (authenticationSchema == VerifyAuthSchemaOptions.VerifyAuthSchema)
            {
                secretKey = _verifyAuthSchemaOptions.SecretKey;
                issuer = _verifyAuthSchemaOptions.Issuer;
            }
            else if (authenticationSchema == MainAuthSchemaOptions.MainAuthSchema)
            {
                secretKey = _mainAuthSchemaOptions.SecretKey;
                issuer = _mainAuthSchemaOptions.Issuer;
            }

            if (secretKey == null || issuer == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            var jwtSecurityToken = new JwtSecurityToken(
                        issuer: issuer,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(10), // Token expiration time
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                            SecurityAlgorithms.HmacSha256)
                        );

            string? accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            if (accessToken == null)
            {
                // TODO: log and throw unhandled exception
                throw new Exception();
            }

            return accessToken;
        }

        public Task GetVerifyJwtAndGetClaimsAsync(string token)
        {
            // Now decode or validate the token
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            throw new NotImplementedException();
        }
    }
}
