namespace TripBooking.Api.Endpoints.Trips;

using ErrorHandling;
using Exceptions;
using FluentValidation;
using Hateoas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Services.Trips;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TripRegistrations;

public static class TripEndpoints
{
    public static async Task<IResult> ListTrips(
        [FromServices] ITripService tripService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trips = await tripService.Get(cancellationToken);

        var response = trips
            .Select(x => x.ToSlimResponse(GenerateLinks(x.Name, linkGenerator, httpContext)))
            .ToArray();

        return Results.Ok(response);
    }
    
    public static async Task<IResult> SearchByCountry(
        [FromQuery(Name = "country"), Required] string country,
        [FromServices] ITripService tripService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trips = await tripService.GetByCountry(country, cancellationToken);
        
        var response = trips
            .Select(x => x.ToSlimResponse(GenerateLinks(x.Name, linkGenerator, httpContext)))
            .ToArray();

        return Results.Ok(response);
    }

    public static async Task<IResult> GetTrip(
        [FromRoute, Required] string name,
        [FromServices] ITripService tripService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trip = await tripService.Get(name, cancellationToken);

        if (trip is null)
        {
            return Results.NotFound();
        }

        var response = new TripResponse
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
        [FromBody, Required] CreateTripRequest request,
        [FromServices] ITripService tripService,
        [FromServices] IValidator<CreateTripRequest> validator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }
        
        var result = await tripService.Create(request.ToDto(), cancellationToken);

        return result.Match<IResult>(
            succ => Results.CreatedAtRoute(
                nameof(GetTrip),
                new { name = succ.Name },
                succ.ToResponse(GenerateLinks(succ.Name, linkGenerator, httpContext))),
            err => Results.BadRequest(err.ToResponse()));
    }

    public static async Task<IResult> UpdateTrip(
        [FromRoute, Required] string name,
        [FromBody, Required] UpdateTripRequest request,
        [FromServices] ITripService tripService,
        [FromServices] IValidator<UpdateTripRequest> validator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }

        var result = await tripService.Update(name, request.ToDto(), cancellationToken);

        return result.Match<IResult>(
            succ => Results.Ok(succ.ToResponse(GenerateLinks(succ.Name, linkGenerator, httpContext))),
            err => err is TripNotFoundException
                ? Results.NotFound()
                : Results.BadRequest(err.ToResponse()));
    }

    public static async Task<IResult> DeleteTrip(
        [FromRoute, Required] string name,
        [FromServices] ITripService tripService,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        await tripService.Delete(name, cancellationToken);

        var response = new TripPostDeleteResponse
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