namespace TripBooking.Api.Endpoints.TripRegistrations;

public record CreateTripRegistrationRequest
{
    public string UserEmail { get; init; } = null!;
}