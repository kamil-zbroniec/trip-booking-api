namespace TripBooking.Api;

using ApplicationServices;
using Carter;
using Domain.Repositories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Middlewares;
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

        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripRegistrationRepository, TripRegistrationRepository>();

        builder.Services
            .AddMediatR(x => x
                .RegisterServicesFromAssemblyContaining<Program>()
                .RegisterServicesFromAssemblyContaining<ApplicationServicesLocator>());
        builder.Services
            .AddValidatorsFromAssemblyContaining<Program>()
            .AddValidatorsFromAssemblyContaining<ApplicationServicesLocator>();
        builder.Services.AddCarter();


        builder.Services.AddDbContext<TripBookingDbContext>(s => s.UseInMemoryDatabase("in-memory-db"));

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.MapCarter();
        app.UseHttpsRedirection();
        
        app.UseMiddleware<ExceptionMiddleware>();
        
        app.Run();
    }
}