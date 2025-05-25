using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application;
using Redress.Backend.Application.Common.Behavior;
using Redress.Backend.Application.Services.ListingArea.Listings;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Infrastructure;
using Redress.Backend.Infrastructure.Persistence;
using System.Text.Json.Serialization;
using MediatR.Extensions.FluentValidation.AspNetCore;
using System.Reflection;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.API.Middleware;
using Redress.Backend.Infrastructure.Integration;

namespace Redress.Backend.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Add Application Services
            builder.Services.AddApplication();

            // Add Persistence Services
            builder.Services.AddPersistence(builder.Configuration);

            var baseDirectoryFromConfig = builder.Configuration["FileStorage:BaseDirectory"];
            var baseDirectory = string.IsNullOrWhiteSpace(baseDirectoryFromConfig)
                ? @"C:\Users\Asus\Desktop\оо\TestsImagesService"
                : baseDirectoryFromConfig;

            builder.Services.AddFileStorage(baseDirectory);

            builder.Services.AddDbContext<RedressDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
            var app = builder.Build();

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
