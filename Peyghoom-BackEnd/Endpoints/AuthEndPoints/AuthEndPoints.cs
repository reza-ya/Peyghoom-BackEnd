using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Peyghoom_BackEnd.Services;

namespace Peyghoom_BackEnd.Endpoints
{
    public static class AuthEndPoints
    {
        public static IEndpointRouteBuilder MapAuthApi(this IEndpointRouteBuilder app, string prefix)
        {
            var group = app.MapGroup(prefix);

            group.MapGet("/login", LoginAsync);
            group.MapGet("/register", Register);

            return app;
        }


        public static async Task<IResult> LoginAsync([FromServices]IAuthService authService)
        {
            await authService.GenerateTokenAsync(new Services.Types.GenerateTokenRequest());
            return Results.Ok(new { Message = "Not Implemented" });
        }

        public static IResult Register()
        {
            return Results.Ok(new { Message = "Not Implemented" });
        }
    }
}
