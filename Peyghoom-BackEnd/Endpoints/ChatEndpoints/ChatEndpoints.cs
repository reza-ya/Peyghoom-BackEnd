using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Peyghoom_BackEnd.AAA;
using Peyghoom_BackEnd.Contracts.types;
using Peyghoom_BackEnd.Infrastructures.Repositories;
using Peyghoom_BackEnd.Services;
using System.Security.Claims;

namespace Peyghoom_BackEnd.Endpoints
{
    [AllowAnonymous]
    public static class ChatEndpoints
    {
        public static IEndpointRouteBuilder MapChatApi(this IEndpointRouteBuilder app, string prefix)
        {
            var group = app.MapGroup(prefix);

            group.MapPost("/openchat", OpenChat);
            group.MapGet("/find-user", FindUser);

            return app;
        }


        public static async Task<IResult> OpenChat([FromBody] OpenChatRequest openChatRequest,
                                        [FromServices] IUserRepository userRepository,
                                        HttpContext httpContext)
        {
            var user = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == MyClaimTypes.UserName)?.Value;
            var userId = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == MyClaimTypes.SubId)?.Value;
            if (user == null || userId == null)
            {
                //TODO: 
                throw new Exception();
            }

            var messages = await userRepository.OpenChatAsync(userId, openChatRequest.UserName);
            return Results.Ok(messages);
        }


        public static IResult FindUser(string query)
        {

            throw new NotImplementedException();
        }


    }
}
