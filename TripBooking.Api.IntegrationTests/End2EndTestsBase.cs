namespace TripBooking.Api.IntegrationTests;

using Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.Http;
using Xunit;

public abstract class End2EndTestsBase : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IConfiguration _configuration;
    private readonly WebApplicationFactory<Program> _factory;

    protected End2EndTestsBase(WebApplicationFactory<Program> factory)
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.Test.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables() 
            .Build();
        
        _factory = factory;
    }

    protected HttpClient GetHttpClient()
    {
        HttpClient client = _factory.CreateClient();
        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ConfigurationErrorsException("Missing api key");
        }
        
        client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, apiKey);
        return client;
    }
}