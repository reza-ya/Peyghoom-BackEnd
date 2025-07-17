using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Peyghoom_BackEnd.Infrastructures;
using Peyghoom_BackEnd.Infrastructures.Repositories;
using Peyghoom_BackEnd.Options;
using Peyghoom_BackEnd.Services;
using System.Text;

namespace Peyghoom_BackEnd
{
    public static class Configurations
    {
        public static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.AddServices();
            builder.AddAuthenticationAndAuthentication();
            builder.AddOptions();
            builder.Services.AddSignalR();
            return builder;
        }



        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddSingleton<IOTPService, OTPService>();
            builder.Services.AddScoped<IPeyghoomContext, PeyghoomContext>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddMemoryCache();
            return builder;
        }


        public static WebApplicationBuilder AddAuthenticationAndAuthentication(this WebApplicationBuilder builder)
        {
            var mainAuthSchemaOptions = builder.Configuration.GetSection(MainAuthSchemaOptions.MainAuthSchema).Get<MainAuthSchemaOptions>();
            var verifyAuthSchemaOptions = builder.Configuration.GetSection(VerifyAuthSchemaOptions.VerifyAuthSchema).Get<VerifyAuthSchemaOptions>();

            if (mainAuthSchemaOptions == null || verifyAuthSchemaOptions == null || mainAuthSchemaOptions.SecretKey == null || mainAuthSchemaOptions.Issuer == null || verifyAuthSchemaOptions.SecretKey == null || verifyAuthSchemaOptions.Issuer == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            builder.Services.AddAuthentication()
                        .AddJwtBearer(MainAuthSchemaOptions.MainAuthSchema, options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = mainAuthSchemaOptions.Issuer,
                                ValidateAudience = false,
                                IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(mainAuthSchemaOptions.SecretKey))
                            };
                        })
                        .AddJwtBearer(VerifyAuthSchemaOptions.VerifyAuthSchema, options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = verifyAuthSchemaOptions.Issuer,
                                ValidateAudience = false,
                                IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(verifyAuthSchemaOptions.SecretKey))
                            };
                        });



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicy.MainPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser().AddAuthenticationSchemes(MainAuthSchemaOptions.MainAuthSchema);
                });

                options.AddPolicy(AuthorizationPolicy.VerifyPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser().AddAuthenticationSchemes(VerifyAuthSchemaOptions.VerifyAuthSchema);
                });
            });

            return builder;
        }


        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<VerifyAuthSchemaOptions>(builder.Configuration.GetSection(VerifyAuthSchemaOptions.VerifyAuthSchema));
            builder.Services.Configure<MainAuthSchemaOptions>(builder.Configuration.GetSection(MainAuthSchemaOptions.MainAuthSchema));
            builder.Services.Configure<PeyghoomContextOptions>(builder.Configuration.GetSection(PeyghoomContextOptions.PeyghoomContext));
            return builder;
        }
    }
}
