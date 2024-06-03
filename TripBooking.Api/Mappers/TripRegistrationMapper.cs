namespace TripBooking.Api.Mappers;

using Domain.Entities;
using Dtos;

public static class TripRegistrationMapper
{
    public static TripRegistrationDto ToDto(this TripRegistrationEntity source) =>
        new()
        {
            TripName = source.TripName,
            UserEmail = source.UserEmail
        };
}