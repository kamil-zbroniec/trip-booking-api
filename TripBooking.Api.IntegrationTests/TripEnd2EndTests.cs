namespace TripBooking.Api.IntegrationTests;

using Endpoints.Trips;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class TripEnd2EndTests : End2EndTestsBase
{
    public TripEnd2EndTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreateTrip_WhenModelIsValid_ShouldReturnCreated()
    {
        var client = GetHttpClient();
        
        try
        {
            // arrange
            var createTripRequest = new CreateTripRequest
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };

            // act
            var response = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Theory]
    [InlineData("", "valid_country", 1)]
    [InlineData("too_long_name_1111111_2222222222_3333333333_4444444444", "valid_country", 1)]
    [InlineData("invalid\nname", "valid_country", 1)]
    [InlineData("valid_name", "", 1)]
    [InlineData("valid_name", "too_long_country_0000", 1)]
    [InlineData("valid_name", "valid_country", 0)]
    [InlineData("valid_name", "valid_country", 101)]
    public async Task CreateTrip_WhenModelIsNotValid_ShouldReturnBadRequest(string name, string country, int numberOfSeats)
    {
        // arrange
        var client = GetHttpClient();

        var createTripRequest = new CreateTripRequest
        {
            Name = name,
            Country = country,
            Description = "valid_description",
            Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
            NumberOfSeats = numberOfSeats
        };

        // act
        var response = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateTrip_WhenModelIsValid_ShouldReturnOk()
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            var createTripRequest = new CreateTripRequest
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };

            _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
        
            var updateTripRequest = new UpdateTripRequest()
            {
                Country = "new_country",
                Description = "new_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };
        
            // act
            var response = await client.PutAsJsonAsync($"api/v1/trip/{createTripRequest.Name}", updateTripRequest);
        
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Fact]
    public async Task UpdateTrip_WhenTripNotExists_ShouldReturnNotFound()
    {
        // arrange
        var client = GetHttpClient();

        var updateTripRequest = new UpdateTripRequest()
        {
            Country = "new_country",
            Description = "new_description",
            Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
            NumberOfSeats = 5
        };
    
        // act
        var response = await client.PutAsJsonAsync($"api/v1/trip/valid_name", updateTripRequest);
    
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("too_long_country_0000", 1)]
    [InlineData("valid_country", 0)]
    [InlineData("valid_country", 101)]
    public async Task UpdateTrip_WhenModelIsNotValid_ShouldReturnBadRequest(string country, int numberOfSeats)
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            var createTripRequest = new CreateTripRequest
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };
        
            var updateTripRequest = new UpdateTripRequest()
            {
                Country = country,
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = numberOfSeats
            };
        
            // act
            var response = await client.PutAsJsonAsync($"api/v1/trip/{createTripRequest.Name}", updateTripRequest);
        
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Fact]
    public async Task DeleteTrip_WhenTripExists_ShouldReturnOk()
    {
        // arrange
        var client = GetHttpClient();
        
        var createTripRequest = new CreateTripRequest
        {
            Name = "valid_name",
            Country = "valid_country",
            Description = "valid_description",
            Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
            NumberOfSeats = 5
        };

        _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
    
        // act
        var response = await client.DeleteAsync("api/v1/trip/valid_name");
    
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteTrip_WhenTripNotExists_ShouldReturnOk()
    {
        // arrange
        var client = GetHttpClient();
        
        // act
        var response = await client.DeleteAsync("api/v1/trip/valid_name");
    
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetTrip_WhenTripExists_ShouldReturnOk()
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            var createTripRequest = new CreateTripRequest
            {
                Name = "valid_name",
                Country = "valid_country",
                Description = "valid_description",
                Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                NumberOfSeats = 5
            };
            
            _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);
        
            // act
            var response = await client.GetAsync($"api/v1/trip/{createTripRequest.Name}");
        
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            _ = await client.DeleteAsync("api/v1/trip/valid_name");
        }
    }
    
    [Fact]
    public async Task GetTrip_WhenTripNotExists_ShouldReturnNotFound()
    {
        // arrange
        var client = GetHttpClient();
    
        // act
        var response = await client.GetAsync($"api/v1/trip/valid_name");
    
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ListTrips_ShouldReturnAllTrips()
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            for (int i = 0; i < 3; i++)
            {
                var createTripRequest = new CreateTripRequest
                {
                    Name = $"valid_name_{i}",
                    Country = "valid_country",
                    Description = "valid_description",
                    Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                    NumberOfSeats = 5
                };
            
                _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);    
            }
        
            // act
            var trips = await client.GetFromJsonAsync<IReadOnlyCollection<TripResponse>>("api/v1/trip/list");
        
            // assert
            trips.Count.Should().Be(3);
        }
        finally
        {
            for (int i = 0; i < 3; i++)
            {
                _ = await client.DeleteAsync($"api/v1/trip/valid_name_{i}");
            }
        }
    }
    
    [Fact]
    public async Task SearchTripsByCountry_ShouldReturnFilteredTrips()
    {
        var client = GetHttpClient();

        try
        {
            // arrange
            for (int i = 0; i < 3; i++)
            {
                var createTripRequest = new CreateTripRequest
                {
                    Name = $"valid_name_{i}",
                    Country = $"valid_country_{i % 2}",
                    Description = "valid_description",
                    Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
                    NumberOfSeats = 5
                };
            
                _ = await client.PostAsJsonAsync("api/v1/trip", createTripRequest);    
            }
        
            // act
            var trips = await client.GetFromJsonAsync<IReadOnlyCollection<TripResponse>>("api/v1/trip/search?country=valid_country_0");
        
            // assert
            trips.Count.Should().Be(2);
            trips.Should().OnlyContain(x => x.Country == "valid_country_0");
        }
        finally
        {
            for (int i = 0; i < 3; i++)
            {
                _ = await client.DeleteAsync($"api/v1/trip/valid_name_{i}");
            }
        }
    }
}