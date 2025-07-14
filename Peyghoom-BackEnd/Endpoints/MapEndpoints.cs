namespace Peyghoom_BackEnd.Endpoints
{
    public static class MapEndpoints
    {
        public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapAuthApi("/auth");

            return app;
        }
    }
}
