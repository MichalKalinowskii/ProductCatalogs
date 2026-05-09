using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ProductCatalogs.Catalogs.Application.Catalogs;
using ProductCatalogs.Catalogs.Application.Interfaces;
using ProductCatalogs.Catalogs.Infrastructure;
using ProductCatalogs.Catalogs.Infrastructure.Repository;
using ProductCatalogs.Products.Application.Interfaces;
using ProductCatalogs.Products.Application.Products;
using ProductCatalogs.Products.Infrastructure;
using ProductCatalogs.Products.Infrastructure.Repository;
using System;

namespace ProductCatalogs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<SqliteConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });

            builder.Services.AddDbContext<ProductDbContext>((sp, options) =>
                options.UseSqlite(sp.GetRequiredService<SqliteConnection>()));

            builder.Services.AddDbContext<CatalogDbContext>((sp, options) =>
                options.UseSqlite(sp.GetRequiredService<SqliteConnection>()));

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICatalogApi, CatalogRepository>();

            builder.Services.AddScoped<CatalogService>();
            builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
            builder.Services.AddScoped<IProductApi, ProductRepository>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<ProductDbContext>()
                    .Database
                    .GetService<IRelationalDatabaseCreator>()
                    .CreateTables();

                scope.ServiceProvider
                    .GetRequiredService<CatalogDbContext>()
                    .Database
                    .GetService<IRelationalDatabaseCreator>()
                    .CreateTables();
            }

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
    }
}
