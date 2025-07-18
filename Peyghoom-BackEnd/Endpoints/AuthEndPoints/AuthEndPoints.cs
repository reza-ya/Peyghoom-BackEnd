using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Peyghoom_BackEnd.AAA;
using Peyghoom_BackEnd.Constants;
using Peyghoom_BackEnd.Contracts.types;
using Peyghoom_BackEnd.Contracts.types.VerificationCode;
using Peyghoom_BackEnd.Options;
using Peyghoom_BackEnd.Services;
using System.Net;
using System.Security.Claims;

namespace Peyghoom_BackEnd.Endpoints
{
    public static class AuthEndPoints
    {
        public static IEndpointRouteBuilder MapAuthApi(this IEndpointRouteBuilder app, string prefix)
        {
            var group = app.MapGroup(prefix);

            group.MapPost("/verification-code", VerificationCode);

            group.MapPost("/verify", Verify)
                    .RequireAuthorization(AuthorizationPolicy.PhoneNumberPolicy);

            return app;
        }   


        public static IResult VerificationCode([FromBody] VerificationCodeRequest verificationCodeRequest, 
                                                                    [FromServices] IOTPService oTPService,
                                                                    [FromServices] IAuthService authService)
        {
            oTPService.SendCode(verificationCodeRequest.PhoneNumber, verificationCodeRequest.CountryCode);

            var token = authService.GenerateToken(new List<Claim>() { new Claim(ClaimKey.PhoneNumber, "true") }, DateTime.UtcNow.AddMinutes(120), MainAuthSchemaOptions.MainAuthSchema);

            return Results.Ok(new { token });
        }


        public static IResult Verify([FromBody] VerifyRequest verifyRequest,
                                            [FromServices] IOTPService oTPService,
                                            [FromServices] IAuthService authService,
                                            HttpContext httpContext)
        {
            long.TryParse(httpContext.User.FindFirst(ClaimKey.PhoneNumber)?.Value, out long phoneNumber);

            if (phoneNumber == 0)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            var result = oTPService.VerifyCode(phoneNumber, verifyRequest.Code);

            
            if (result.IsFailure)
            {
                return Results.StatusCode((int)HttpStatusCode.Gone);
            }


            var token = authService.GenerateToken(new List<Claim>() { }, DateTime.UtcNow.AddHours(12), MainAuthSchemaOptions.MainAuthSchema);

            return Results.Ok(new { token });
        }
    }
}
