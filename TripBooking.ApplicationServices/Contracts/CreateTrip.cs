namespace TripBooking.ApplicationServices.Contracts;

using System;

public record CreateTrip
{
    public string Name { get; init; } = null!;

    public string Country { get; init; } = null!;

    public string Description { get; init; } = string.Empty;
    
    public DateTime Start { get; init; }
    
    public int NumberOfSeats { get; init; }
}