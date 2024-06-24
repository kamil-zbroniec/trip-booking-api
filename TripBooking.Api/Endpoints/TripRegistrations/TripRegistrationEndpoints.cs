namespace TripBooking.Api.Endpoints.TripRegistrations;

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
using System.Threading;
using System.Threading.Tasks;
using Trips;

public static class TripRegistrationEndpoints
{
    public static async Task<IResult> GetTripRegistration(
        [FromRoute, Required] string name,
        [FromRoute, Required] string email,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<CreateTripRegistrationRequest> validator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var trip = await mediator.Send(new GetTripRegistrationRequest(name, email), cancellationToken);
        
        if (trip is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(trip.ToResponse(GenerateLinks(trip.TripName, trip.UserEmail, linkGenerator, httpContext)));
    }
    
    public static async Task<IResult> CreateTripRegistration(
        [FromRoute, Required] string name,
        [FromBody, Required] CreateTripRegistrationRequest request,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<CreateTripRegistrationRequest> validator,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }
        
        var result = await mediator.Send(new RegisterForTripRequest(name, request.ToDto()), cancellationToken);
        
        return result.Match<IResult>(
            succ => Results.CreatedAtRoute(
                nameof(GetTripRegistration),
                new { name = succ.TripName, email = succ.UserEmail },
                succ.ToResponse(GenerateLinks(succ.TripName, succ.UserEmail, linkGenerator, httpContext))),
            err => err is TripNotFound
                ? Results.NotFound()
                : Results.BadRequest(err.Message));
    }
    
    private static Link[] GenerateLinks(string name, string email, LinkGenerator linkGenerator, HttpContext httpContext)
    {
        return
        [
            new Link(linkGenerator.GetUriByName(httpContext, nameof(GetTripRegistration), values: new {name, email})!, "self", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(CreateTripRegistration), values: new {name})!, "register-for-a-trip", HttpMethod.Post),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(TripEndpoints.ListTrips))!, "list-trips", HttpMethod.Get),
            new Link(linkGenerator.GetUriByName(httpContext, nameof(TripEndpoints.SearchByCountry))!, "search-by-country", HttpMethod.Get)
        ];
    }
}