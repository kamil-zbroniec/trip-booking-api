namespace TripBooking.Api.Authentication;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyEndpointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var apiKey))
        {
            return TypedResults.Unauthorized();
        }

        var expectedApiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName)!;
        if (!expectedApiKey.Equals(apiKey))
        {
            return TypedResults.Unauthorized();
        }

        return await next(context);
    }
}