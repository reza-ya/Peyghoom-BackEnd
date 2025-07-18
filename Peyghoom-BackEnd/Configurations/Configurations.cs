using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Peyghoom_BackEnd.AAA;
using Peyghoom_BackEnd.Exceptions;
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
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
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

            if (mainAuthSchemaOptions == null || mainAuthSchemaOptions.SecretKey == null || mainAuthSchemaOptions.Issuer == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = mainAuthSchemaOptions.Issuer,
                                ValidateAudience = false,
                                IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(mainAuthSchemaOptions.SecretKey))
                            };
                        });
            //.AddJwtBearer(VerifyAuthSchemaOptions.VerifyAuthSchema, options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = verifyAuthSchemaOptions.Issuer,
            //        ValidateAudience = false,
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(verifyAuthSchemaOptions.SecretKey))
            //    };
            //});



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicy.PhoneNumberPolicy, policy =>
                {
                    policy.Requirements.Add(new ClaimRequirement(ClaimTypes.PhoneNumber));
                    //policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser();
                });

                options.AddPolicy(AuthorizationPolicy.PhoneVerifiedPolicy, policy =>
                {
                    policy.Requirements.Add(new ClaimRequirement(ClaimTypes.PhoneVerified));
                    //policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser();
                });

                options.AddPolicy(AuthorizationPolicy.RegisteredPolicy, policy =>
                {
                    policy.Requirements.Add(new ClaimRequirement(ClaimTypes.UserRegisterd));
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser();
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
