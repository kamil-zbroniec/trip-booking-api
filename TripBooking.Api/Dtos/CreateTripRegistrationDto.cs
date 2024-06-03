namespace TripBooking.Api.Dtos;

public record CreateTripRegistrationDto
{
    public string UserEmail { get; init; } = null!;
}