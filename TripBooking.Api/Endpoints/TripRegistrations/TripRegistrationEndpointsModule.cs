namespace TripBooking.Api.Endpoints.TripRegistrations;

using Authentication;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;

public class TripRegistrationEndpointsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/trip/{name}/registration")
            .AddEndpointFilter<ApiKeyEndpointFilter>();

        group.MapGet("{email}", TripRegistrationEndpoints.GetTripRegistration)
            .WithName(nameof(TripRegistrationEndpoints.GetTripRegistration))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Get trip registration by name and user email" })
            .Produces<TripRegistrationResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", TripRegistrationEndpoints.CreateTripRegistration)
            .WithName(nameof(TripRegistrationEndpoints.CreateTripRegistration))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Create for a trip" })
            .Produces<TripRegistrationResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}