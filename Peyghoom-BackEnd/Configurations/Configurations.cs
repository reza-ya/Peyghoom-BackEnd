using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            var jwtOptions = builder.Configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();

            if (jwtOptions == null || jwtOptions.AccessTokenSecretKey == null || jwtOptions.Issuer == null)
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
                                ValidIssuer = jwtOptions.Issuer,
                                ValidateAudience = false,
                                IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtOptions.AccessTokenSecretKey))
                            };
                        });



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(MyAuthorizationPolicy.PhoneNumberPolicy, policy =>
                {
                    policy.Requirements.Add(new ClaimRequirement(MyClaimTypes.PhoneNumber));
                });

                options.AddPolicy(MyAuthorizationPolicy.PhoneVerifiedPolicy, policy =>
                {
                    policy.Requirements.Add(new ClaimRequirement(MyClaimTypes.PhoneVerified));
                });

                options.AddPolicy(MyAuthorizationPolicy.RegisteredPolicy, policy =>
                {
                    policy.Requirements.Add(new ClaimRequirement(MyClaimTypes.UserRegisterd));
                });
            });

            builder.Services.AddSingleton<IAuthorizationHandler, ClaimRequirementHandler>();

            return builder;
        }


        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<VerifyAuthSchemaOptions>(builder.Configuration.GetSection(VerifyAuthSchemaOptions.VerifyAuthSchema));
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Jwt));
            builder.Services.Configure<PeyghoomContextOptions>(builder.Configuration.GetSection(PeyghoomContextOptions.PeyghoomContext));
            return builder;
        }
    }
}
