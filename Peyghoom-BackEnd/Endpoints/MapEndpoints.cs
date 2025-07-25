namespace Peyghoom_BackEnd.Endpoints
{
    public static class MapEndpoints
    {
        public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapAuthApi("/auth");
            app.MapChatApi("/chat");

            return app;
        }
    }
}
