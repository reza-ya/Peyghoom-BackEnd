
using Peyghoom_BackEnd.Endpoints;
using Peyghoom_BackEnd.Hubs;

namespace Peyghoom_BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.AddConfigurations();

                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
                {
                    app.MapOpenApi();
                    app.UseExceptionHandler("/Error");
                }

                app.UseHttpsRedirection();

                app.UseCors("cors");

                app.UseAuthentication();
                app.UseAuthorization();


                app.MapAllEndpoints();
                app.MapHub<ChatHub>("/chatHub");

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Application fail to start");
                Console.WriteLine(ex.ToString());

                Environment.Exit(0);
            }
        }
    }
}
