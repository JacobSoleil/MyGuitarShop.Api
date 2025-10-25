
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using MyGuitarShop.Data.Ado.Factories;

namespace MyGuitarShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddLogging(builder);

            AddServices(builder);

            var connectionString = builder.Configuration.GetConnectionString("MyGuitarShop");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void AddLogging(WebApplicationBuilder builder)
        {
            builder.Services.AddLogging(logging => 
            { 
                logging.ClearProviders();
                logging
                .AddFilter("Microsoft", LogLevel.Information)
                .AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information)
                .AddConsole();
            });

            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPath
                                        | HttpLoggingFields.RequestMethod
                                        | HttpLoggingFields.ResponseStatusCode;
            });
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("MyGuitarShop")
                ?? throw new InvalidOperationException("MyGuitarShop connection string not found.");

            builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

            builder.Services.AddControllers();
        }
    }
}
