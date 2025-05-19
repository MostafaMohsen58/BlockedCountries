
using BlockedCountries.Repositories;
using BlockedCountries.Repositories.interfaces;
using BlockedCountries.Services;
using BlockedCountries.Services.Background;
using BlockedCountries.Services.Interfaces;

namespace BlockedCountries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            //register my services
            builder.Services.AddHttpClient<IIpGeolocationService, IpGeolocationService>();
            builder.Services.AddSingleton<ICountryBlockingRepository, CountryBlockingRepository>();
            builder.Services.AddSingleton<IBlockedAttemptsLoggerRepository, BlockedAttemptsLoggerRepository>();

            builder.Services.AddHostedService<TemporalBlockCleanupService>();


            var app = builder.Build();



            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
