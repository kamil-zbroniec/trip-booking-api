namespace TripBooking.Api.Endpoints.TripRegistrations;

public record CreateTripRegistrationRequestModel
{
    public string UserEmail { get; init; } = null!;
}