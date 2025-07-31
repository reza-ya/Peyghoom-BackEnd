using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peyghoom_BackEnd.AAA;
using Peyghoom_BackEnd.Contracts.types;
using Peyghoom_BackEnd.Entities;
using Peyghoom_BackEnd.Infrastructures.Repositories;
using Peyghoom_BackEnd.Services;
using Peyghoom_BackEnd.Services.Types;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Peyghoom_BackEnd.Endpoints
{
    public static class AuthEndPoints
    {
        public static IEndpointRouteBuilder MapAuthApi(this IEndpointRouteBuilder app, string prefix)
        {
            var group = app.MapGroup(prefix);

            group.MapPost("/verification-code", VerificationCode);

            group.MapPost("/verification-code/verify", Verify)
                    .RequireAuthorization(MyAuthorizationPolicy.PhoneNumberPolicy);

            group.MapPost("/refresh", Refresh);

            group.MapPost("/register", Register)
                    .RequireAuthorization(MyAuthorizationPolicy.PhoneVerifiedPolicy);

            return app;
        }   


        public static IResult VerificationCode([FromBody] VerificationCodeRequest verificationCodeRequest, 
                                                                    [FromServices] IOTPService oTPService,
                                                                    [FromServices] IAuthService authService)
        {
            oTPService.SendCode(verificationCodeRequest.PhoneNumber, verificationCodeRequest.CountryCode);

            var token = authService.GenerateToken(
                                    new List<Claim>() { new Claim(MyClaimTypes.PhoneNumber, verificationCodeRequest.PhoneNumber.ToString())}, 
                                    DateTime.UtcNow.AddMinutes(2));

            return Results.Ok(new { token });
        }


        public static async Task<IResult> Verify([FromBody] VerifyRequest verifyRequest,
                                                [FromServices] IOTPService oTPService,
                                                [FromServices] IAuthService authService,
                                                [FromServices] IUserRepository userRepository,
                                                HttpContext httpContext)
        {
            long.TryParse(httpContext.User.FindFirst(MyClaimTypes.PhoneNumber)?.Value, out long phoneNumber);

            if (phoneNumber == 0)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            //var result = oTPService.VerifyCode(phoneNumber, verifyRequest.Code);

            
            //if (result.IsFailure)
            //{
            //    return Results.StatusCode((int)HttpStatusCode.Gone);
            //}

            var users = await userRepository.GetAllUsersAsync();
            var user = users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);
            if (user != null)
            {
                var jwtTokens = authService.GenerateTokenRefreshToken(
                                            new List<Claim>() {
                                                new Claim(MyClaimTypes.UserRegisterd , "true"),
                                                new Claim(MyClaimTypes.UserName, user.Username),
                                                new Claim(MyClaimTypes.SubId, user.Id.ToString())
                                            }, 
                                            DateTime.UtcNow.AddHours(10)
                                            ,DateTime.UtcNow.AddDays(15));

                return Results.Ok(new VerifyResponse { AccessToken = jwtTokens.AccessToken, RefreshToken = jwtTokens.RefreshToken });
            }

            var token = authService.GenerateToken(
                                            new List<Claim>() 
                                            {
                                                new Claim(MyClaimTypes.PhoneVerified, "true"),
                                                new Claim(MyClaimTypes.PhoneNumber, phoneNumber.ToString())
                                            },
                                            DateTime.UtcNow.AddMinutes(30));

            return Results.Ok(new VerifyResponse { AccessToken = token });
        }


        public static async Task<IResult> Register([FromBody] RegisterRequest registerRequest,
                                        [FromServices] IAuthService authService,
                                        [FromServices] IUserRepository userRepository,
                                        HttpContext httpContext)
        {
            long.TryParse(httpContext.User.FindFirst(MyClaimTypes.PhoneNumber)?.Value, out long phoneNumber);

            if (phoneNumber == 0)
            {
                // TODO: log and throw exception
                throw new Exception();
            }
            var newUser = new Users() 
            { 
                FirstName = registerRequest.FirstName, 
                LastName = registerRequest.LastName,
                PhoneNumber = phoneNumber,
                Username = registerRequest.UserName,
            };

            var doesUserExist = await userRepository.DoesUserExistAsync(newUser);
            if (doesUserExist == true)
            {
                return Results.BadRequest(new { Message = "username already taken"});
            }

            string UserId =await userRepository.InsertUserAsync(newUser);

            var jwtTokens = authService.GenerateTokenRefreshToken(
                            new List<Claim>() 
                            {
                                new Claim(MyClaimTypes.UserRegisterd, "true"),
                                new Claim(MyClaimTypes.UserName, UserId)
                            },
                            DateTime.UtcNow.AddHours(1)
                            , DateTime.UtcNow.AddDays(15));

            return Results.Ok(new RegisterResponse { AccessToken = jwtTokens.AccessToken, RefreshToken = jwtTokens.RefreshToken });
        }

        public static IResult Refresh([FromBody] RefreshRequest refreshRequest,
                                       [FromServices] IAuthService authService)
        {

            var principal = authService.GetPrincipalAndValidateRefreshToken(refreshRequest.RefreshToken);
            if (principal == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }
            var jwtTokens = authService.GenerateTokenRefreshToken(
                            principal.Claims.ToList(),
                            DateTime.UtcNow.AddHours(1)
                            , DateTime.UtcNow.AddDays(15));

            return Results.Ok(new RefreshResponse { AccessToken = jwtTokens.AccessToken, RefreshToken = jwtTokens.RefreshToken });
        }
    }
}
