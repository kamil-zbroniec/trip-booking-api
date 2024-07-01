namespace TripBooking.Api.Endpoints.Trips;

using ApplicationServices.Errors;
using ApplicationServices.Requests;
using FluentValidation;
using Hateoas;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TripRegistrations;

public static class TripEndpoints
{
    public static async Task<IResult> ListTrips(
        [FromServices] IMediator mediator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trips = await mediator.Send(new GetTripsRequest(), cancellationToken);

        var response = trips
            .Select(x => x.ToSlimResponse(GenerateLinks(x.Name, linkGenerator, httpContext)))
            .ToArray();

        return Results.Ok(response);
    }
    
    public static async Task<IResult> SearchByCountry(
        [FromQuery(Name = "country"), Required] string country,
        [FromServices] IMediator mediator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trips = await mediator.Send(new GetTripsByCountryRequest(country), cancellationToken);
        
        var response = trips
            .Select(x => x.ToSlimResponse(GenerateLinks(x.Name, linkGenerator, httpContext)))
            .ToArray();

        return Results.Ok(response);
    }

    public static async Task<IResult> GetTrip(
        [FromRoute, Required] string name,
        [FromServices] IMediator mediator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trip = await mediator.Send(new GetTripByNameRequest(name), cancellationToken);

        if (trip is null)
        {
            return Results.NotFound();
        }

        var response = new TripResponseModel
        {
            Name = trip.Name,
            Country = trip.Country,
            Description = trip.Description,
            Start = trip.Start,
            NumberOfSeats = trip.NumberOfSeats,
            Links = GenerateLinks(name, linkGenerator, httpContext)
        };
                
        return Results.Ok(response);
    }
    
    public static async Task<IResult> CreateTrip(
        [FromBody, Required] CreateTripRequestModel request,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<CreateTripRequestModel> validator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }

        var result = await mediator.Send(new CreateTripRequest(request.ToDto()), cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                nameof(GetTrip),
                new { name = result.Value.Name },
                result.Value.ToResponse(GenerateLinks(result.Value.Name, linkGenerator, httpContext)))
            : Results.BadRequest(result.Error.Description);
    }

    public static async Task<IResult> UpdateTrip(
        [FromRoute, Required] string name,
        [FromBody, Required] UpdateTripRequestModel request,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<UpdateTripRequestModel> validator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }
        
        var result = await mediator.Send(new UpdateTripRequest(name, request.ToDto()), cancellationToken);
        
        return result.IsSuccess
            ? Results.Ok(result.Value.ToResponse(GenerateLinks(result.Value.Name, linkGenerator, httpContext)))
            : result.Error == DomainErrors.Trip.NotFound
                ? Results.NotFound()
                : Results.BadRequest(result.Error.Description);
    }

    public static async Task<IResult> DeleteTrip(
        [FromRoute, Required] string name,
        [FromServices] IMediator mediator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteTripRequest(name), cancellationToken);

        var response = new TripPostDeleteResponseModel
        {
            Links = GeneratePostDeleteLinks(linkGenerator, httpContext)
        };
        
        return Results.Ok(response);
    }
    
    private static Link[] GenerateLinks(string name, LinkGenerator linkGenerator, HttpContext httpContext)
    {
        return
        [
            new Link(linkGenerator.GetUriByName(httpContext, nameof(GetTrip), values: new {name})!, "self", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(UpdateTrip), values: new {name})!, "update-trip", HttpMethod.Put),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(DeleteTrip), values: new {name})!, "delete-trip", HttpMethod.Delete),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(TripRegistrationEndpoints.CreateTripRegistration), values: new {name})!, "register-for-a-trip", HttpMethod.Post),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(ListTrips))!, "list-trips", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(SearchByCountry))!, "search-by-country", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(CreateTrip))!, "create-trip", HttpMethod.Post)
        ];
    }
    
    private static Link[] GeneratePostDeleteLinks(LinkGenerator linkGenerator, HttpContext httpContext)
    {
        return
        [
            new Link(linkGenerator.GetUriByName(httpContext, nameof(ListTrips))!, "list-trips", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(SearchByCountry))!, "search-by-country", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(CreateTrip))!, "create-trip", HttpMethod.Post)
        ];
    }
}