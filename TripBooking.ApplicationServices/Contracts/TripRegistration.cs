namespace TripBooking.ApplicationServices.Contracts;

public record TripRegistration
{
    public string TripName { get; init; } = null!;

    public string UserEmail { get; init; } = null!;
}