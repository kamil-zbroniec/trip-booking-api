namespace TripBooking.Api.Endpoints.TripRegistrations;

using Dtos;
using Hateoas;
using System.Collections.Generic;

public static class TripRegistrationMapper
{
    public static CreateTripRegistrationDto ToDto(this CreateTripRegistrationRequest source) =>
        new()
        {
            UserEmail = source.UserEmail
        };
    
    public static TripRegistrationResponse ToResponse(this TripRegistrationDto source, IReadOnlyCollection<Link> links) =>
        new()
        {
            TripName = source.TripName,
            UserEmail = source.UserEmail,
            Links = links
        };
}