namespace TripBooking.Api.IntegrationTests;

using Endpoints.TripRegistrations;
using Endpoints.Trips;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class TripRegistrationEnd2EndTests : End2EndTestsBase
{
    public TripRegistrationEnd2EndTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreateTripRegistration_WhenModelIsValid_ShouldReturnCreated()
    {
        var client = GetHttpClient();
        
        try
        {
            // arrange
            var createTripRequest = new CreateTripRequestModel
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };

            _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
            
            var createTripRegistrationRequest = new CreateTripRegistrationRequestModel
            {
                UserEmail = "user@email.com"
            };
            
            // act
            var response = await client.PostAsJsonAsync("api/v1/trip/valid_name/registration", createTripRegistrationRequest);
            
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Fact]
    public async Task CreateTripRegistration_WhenTripNotExists_ShouldReturnNotFound()
    {
        // arrange
        var client = GetHttpClient();
        
        var createTripRegistrationRequest = new CreateTripRegistrationRequestModel
        {
            UserEmail = "user@email.com"
        };
        
        // act
        var response = await client.PostAsJsonAsync("api/v1/trip/valid_name/registration", createTripRegistrationRequest);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateTripRegistration_WhenEmailIsNotValid_ShouldReturnBadRequest()
    {
        var client = GetHttpClient();
        
        try
        {
            // arrange
            var createTripRequest = new CreateTripRequestModel
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };

            _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
            
            var createTripRegistrationRequest = new CreateTripRegistrationRequestModel
            {
                UserEmail = "user@mail"
            };
            
            // act
            var response = await client.PostAsJsonAsync("api/v1/trip/valid_name/registration", createTripRegistrationRequest);
            
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Fact]
    public async Task GetTripRegistration_WhenTripRegistrationExists_ShouldReturnOk()
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            var createTripRequest = new CreateTripRequestModel
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };
            
            _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
            
            var createTripRegistrationRequest = new CreateTripRegistrationRequestModel
            {
                UserEmail = "user@mail.com"
            };
            
            _ = await client.PostAsJsonAsync("api/v1/trip/valid_name/registration", createTripRegistrationRequest);
        
            // act
            var response = await client.GetAsync($"api/v1/trip/{createTripRequest.Name}/registration/{createTripRegistrationRequest.UserEmail}");
        
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Fact]
    public async Task GetTripRegistration_WhenTripRegistrationNotExists_ShouldReturnNotFound()
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            var createTripRequest = new CreateTripRequestModel
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };
            
            _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
        
            // act
            var response = await client.GetAsync($"api/v1/trip/{createTripRequest.Name}/registration/user@mail.com");
        
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
}