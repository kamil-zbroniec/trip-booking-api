namespace TripBooking.Api.Endpoints.ErrorHandling;

public record ErrorResponse
{
    public string Message { get; init; }
}