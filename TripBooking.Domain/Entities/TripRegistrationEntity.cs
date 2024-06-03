namespace TripBooking.Domain.Entities;

public record TripRegistrationEntity
{
    public string TripName { get; init; } = null!;

    public string UserEmail { get; init; } = null!;

    public TripEntity Trip { get; init; } = null!;
}