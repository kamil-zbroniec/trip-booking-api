namespace TripBooking.Api.Endpoints.Trips;

using Authentication;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

public class TripEndpointsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/trip")
            .AddEndpointFilter<ApiKeyEndpointFilter>();

        group.MapGet("list", TripEndpoints.ListTrips)
            .WithName(nameof(TripEndpoints.ListTrips))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "List all trips" })
            .Produces<IReadOnlyCollection<TripResponse>>()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("search", TripEndpoints.SearchByCountry)
            .WithName(nameof(TripEndpoints.SearchByCountry))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Search by country" })
            .Produces<IReadOnlyCollection<TripResponse>>()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("{name}", TripEndpoints.GetTrip)
            .WithName(nameof(TripEndpoints.GetTrip))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Get trip by name" })
            .Produces<TripResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", TripEndpoints.CreateTrip)
            .WithName(nameof(TripEndpoints.CreateTrip))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Create a trip", })
            .Produces<TripResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("{name}", TripEndpoints.UpdateTrip)
            .WithName(nameof(TripEndpoints.UpdateTrip))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Update trip by name", })
            .Produces<TripResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("{name}", TripEndpoints.DeleteTrip)
            .WithName(nameof(TripEndpoints.DeleteTrip))
            .WithMetadata(new SwaggerOperationAttribute { Summary = "Delete trip by name" })
            .Produces<TripPostDeleteResponse>();
    }
}