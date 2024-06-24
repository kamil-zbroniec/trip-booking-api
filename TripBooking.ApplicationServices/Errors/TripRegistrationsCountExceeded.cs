namespace TripBooking.ApplicationServices.Errors;

public struct TripRegistrationsCountExceeded : IOperationError
{
    public string Message { get; }

    public TripRegistrationsCountExceeded(string tripName)
    {
        Message = $"Trip {tripName} has already reached its maximum registrations count";
    }
}