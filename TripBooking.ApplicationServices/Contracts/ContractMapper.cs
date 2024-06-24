namespace TripBooking.ApplicationServices.Contracts;

using Domain.Entities;

public static class ContractMapper
{
    public static Trip ToDto(this TripEntity source) =>
        new()
        {
            Name = source.Name,
            Country = source.Country,
            Description = source.Description,
            Start = source.Start,
            NumberOfSeats = source.NumberOfSeats
        };
    
    public static TripRegistration ToDto(this TripRegistrationEntity source) =>
        new()
        {
            TripName = source.TripName,
            UserEmail = source.UserEmail
        };
}