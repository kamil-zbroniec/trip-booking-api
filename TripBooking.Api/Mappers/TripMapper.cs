namespace TripBooking.Api.Mappers;

using Domain.Entities;
using Dtos;

public static class TripMapper
{
    public static TripDto ToDto(this TripEntity source) =>
        new()
        {
            Name = source.Name,
            Country = source.Country,
            Description = source.Description,
            Start = source.Start,
            NumberOfSeats = source.NumberOfSeats,
        };
}