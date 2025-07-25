using Microsoft.AspNetCore.Mvc;
using Peyghoom_BackEnd.AAA;
using Peyghoom_BackEnd.Contracts.types;
using Peyghoom_BackEnd.Services;
using System.Security.Claims;

namespace Peyghoom_BackEnd.Endpoints
{
    public static class ChatEndpoints
    {
        public static IEndpointRouteBuilder MapChatApi(this IEndpointRouteBuilder app, string prefix)
        {
            var group = app.MapGroup(prefix);

            group.MapPost("/openchat", OpenChat);
            group.MapGet("/find-user", FindUser);

            return app;
        }


        public static IResult OpenChat([FromBody] OpenChatRequest openChatRequest)
        {

            throw new NotImplementedException();
        }


        public static IResult FindUser(string query)
        {

            throw new NotImplementedException();
        }


    }
}
