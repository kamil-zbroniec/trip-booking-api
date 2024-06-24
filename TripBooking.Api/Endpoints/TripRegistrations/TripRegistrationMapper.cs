namespace TripBooking.Api.Endpoints.TripRegistrations;

using ApplicationServices.Contracts;
using Hateoas;
using System.Collections.Generic;

public static class TripRegistrationMapper
{
    public static CreateTripRegistration ToDto(this CreateTripRegistrationRequest source) =>
        new()
        {
            UserEmail = source.UserEmail
        };
    
    public static TripRegistrationResponse ToResponse(this TripRegistration source, IReadOnlyCollection<Link> links) =>
        new()
        {
            TripName = source.TripName,
            UserEmail = source.UserEmail,
            Links = links
        };
}