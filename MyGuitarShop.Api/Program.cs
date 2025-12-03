
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repositories;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Common.DTOs;
using System.Diagnostics;
using MyGuitarShop.Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MyGuitarShop.Data.MongoDb.Services;

namespace MyGuitarShop.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                AddLogging(builder);

                AddServices(builder);

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                if (builder.Environment.IsDevelopment())
                {
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                }

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                ConfigureApplication(app);

                app.Run();
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached) Debugger.Break();
                Console.WriteLine(ex.Message);
            }
        }

        private static void ConfigureApplication(WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
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
            //Ado
            var connectionString = builder.Configuration.GetConnectionString("MyGuitarShop")
                ?? throw new InvalidOperationException("MyGuitarShop connection string not found.");

            builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

            builder.Services.AddScoped<IRepository<AddressDto, int>, AddressRepo>();

            builder.Services.AddScoped<IRepository<AdministratorDto, int>, AdministratorRepo>();

            builder.Services.AddScoped<IUniqueRepository<CategoryDto>, CategoryRepo>();

            builder.Services.AddScoped<IUniqueRepository<CustomerDto>, CustomerRepo>();

            builder.Services.AddScoped<IRepository<OrderDto, int>, OrderRepo>();

            builder.Services.AddScoped<IRepository<OrderItemDto, int>, OrderItemRepo>();

            builder.Services.AddScoped<IUniqueRepository<ProductDto>, ProductRepo>();

            //EFCore
            builder.Services.AddDbContextFactory<MyGuitarShopContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.ProductRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.CategoryRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.AddressRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.CustomerRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.OrderRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.OrderItemRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.AdministratorRepository>();

            //MongoDb
            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb")
                ?? throw new InvalidOperationException("MongoDb connection string not found.");

            builder.Services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoConnectionString));

            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                return mongoClient.GetDatabase("MyGuitarShopCluster");
            });

            builder.Services.AddScoped<MongoProductService>();

            builder.Services.AddControllers();
        }
    }
}
