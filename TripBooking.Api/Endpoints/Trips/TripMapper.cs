namespace TripBooking.Api.Endpoints.Trips;

using ApplicationServices.Contracts;
using Hateoas;
using System.Collections.Generic;

public static class TripMapper
{
    public static CreateTrip ToDto(this CreateTripRequest source) =>
        new()
        {
            Name = source.Name,
            Country = source.Country,
            Description = source.Description,
            Start = source.Start,
            NumberOfSeats = source.NumberOfSeats,
        };
    
    public static UpdateTrip ToDto(this UpdateTripRequest source) =>
        new()
        {
            Country = source.Country,
            Description = source.Description,
            Start = source.Start,
            NumberOfSeats = source.NumberOfSeats,
        };
    
    public static TripResponse ToResponse(this Trip source, IReadOnlyCollection<Link> links) =>
        new()
        {
            Name = source.Name,
            Country = source.Country,
            Description = source.Description,
            Start = source.Start,
            NumberOfSeats = source.NumberOfSeats,
            Links = links
        };
    
    public static TripResponse ToSlimResponse(this Trip source, IReadOnlyCollection<Link> links) =>
        new()
        {
            Name = source.Name,
            Country = source.Country,
            Description = null,
            Start = source.Start,
            NumberOfSeats = default,
            Links = links
        };
}