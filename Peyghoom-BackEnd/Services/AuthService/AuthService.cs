
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Peyghoom_BackEnd.Options;
using Peyghoom_BackEnd.Services.Types;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Peyghoom_BackEnd.Services
{
    public class AuthService : IAuthService
    {
        private JwtOptions _jwtOptions;
        public AuthService(IOptionsSnapshot<JwtOptions> mainOptionsSnapshot)
        {
            _jwtOptions = mainOptionsSnapshot.Value;

        }

        public string GenerateToken(List<Claim> claims, DateTime datetime)
        {
            if (_jwtOptions.AccessTokenSecretKey == null || _jwtOptions.Issuer == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            var jwtSecurityToken = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        claims: claims,
                        expires: datetime, // Token expiration time
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.AccessTokenSecretKey)),
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

        public JwtTokens GenerateTokenRefreshToken(List<Claim> claims, DateTime accessTokenExpirationDateTime, DateTime RefreshTokenExpirationDateTime)
        {
            if (_jwtOptions.AccessTokenSecretKey == null || _jwtOptions.Issuer == null || _jwtOptions.RefreshTokenSecretKey == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }


            var jwtSecurityToken = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        claims: claims,
                        expires: accessTokenExpirationDateTime, // Token expiration time
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.AccessTokenSecretKey)),
                            SecurityAlgorithms.HmacSha256)
                        );

            var jwtSecurityRefreshToken = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        claims: claims,
                        expires: RefreshTokenExpirationDateTime, // Token expiration time
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.RefreshTokenSecretKey)),
                            SecurityAlgorithms.HmacSha256)
                        );

            string? accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            string? refreshToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityRefreshToken);

            if (accessToken == null)
            {
                // TODO: log and throw unhandled exception
                throw new Exception();
            }



            return new JwtTokens() { AccessToken = accessToken, RefreshToken = refreshToken };
        }



        public ClaimsPrincipal? GetPrincipalAndValidateRefreshToken(string token)
        {
            if (_jwtOptions.RefreshTokenSecretKey == null || _jwtOptions.Issuer == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.RefreshTokenSecretKey)),
            };
                
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return principal;
        }
    }
}
