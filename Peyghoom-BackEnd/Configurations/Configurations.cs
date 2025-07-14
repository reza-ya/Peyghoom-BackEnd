using Peyghoom_BackEnd.Services;

namespace Peyghoom_BackEnd
{
    public static class Configurations
    {
        public static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.AddServices();
            return builder;
        }



        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            return builder;
        }
    }
}
