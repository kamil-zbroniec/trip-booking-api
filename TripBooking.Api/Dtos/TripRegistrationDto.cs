namespace TripBooking.Api.Dtos;

public record TripRegistrationDto
{
    public string TripName { get; init; } = null!;

    public string UserEmail { get; init; } = null!;
}