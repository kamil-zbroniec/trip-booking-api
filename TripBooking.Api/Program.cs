// TODO: 
    // - Authorize
    // - Unit tests
    // - Update or remove Swagger Description
// - Deploy
    // - Add Carter (optionally)
// - Reorganize mappers
    // - Remove unused classes
    // - Add registration limit

namespace TripBooking.Api;

using Carter;
using Domain.Repositories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Middlewares;
using Services.TripRegistrations;
using Services.Trips;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.EnableAnnotations();
            x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key needed to access the API",
                Name = "X-Api-Key",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "ApiKey",
                    Type = ReferenceType.SecurityScheme,
                },
                In = ParameterLocation.Header
            };
            
            var requirement = new OpenApiSecurityRequirement
            {
                { scheme, new List<string>() }
            }
                ;
            x.AddSecurityRequirement(requirement);

        });

        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<ITripRegistrationService, TripRegistrationService>();

        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripRegistrationRepository, TripRegistrationRepository>();

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddCarter();

        builder.Services.AddDbContext<TripBookingDbContext>(s => s.UseInMemoryDatabase("in-memory-db"));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapCarter();
        app.UseHttpsRedirection();
        
        app.UseMiddleware<ExceptionMiddleware>();
        
        app.Run();
    }
}