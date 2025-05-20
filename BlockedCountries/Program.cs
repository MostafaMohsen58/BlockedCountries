
using BlockedCountries.Repositories;
using BlockedCountries.Repositories.interfaces;
using BlockedCountries.Services;
using BlockedCountries.Services.Background;
using BlockedCountries.Services.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BlockedCountries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Country Blocking API",
                    Version = "v1",
                    Description = "API for managing blocked countries and IP verification"
                });

                // Enable XML comments
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //register my services
            builder.Services.AddHttpClient<IIpGeolocationService, IpGeolocationService>();
            builder.Services.AddSingleton<ICountryBlockingRepository, CountryBlockingRepository>();
            builder.Services.AddSingleton<IBlockedAttemptsLoggerRepository, BlockedAttemptsLoggerRepository>();

            builder.Services.AddHostedService<TemporalBlockCleanupService>();

            var app = builder.Build();

            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Country Blocking API v1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
