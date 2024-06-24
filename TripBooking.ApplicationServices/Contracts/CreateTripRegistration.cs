namespace TripBooking.ApplicationServices.Contracts;

public record CreateTripRegistration
{
    public string UserEmail { get; init; } = null!;
}