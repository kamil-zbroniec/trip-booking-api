namespace TripBooking.ApplicationServices.Errors;

public struct TripNotFound : IOperationError
{
    public string Message { get; }

    public TripNotFound(string tripName)
    {
        Message = $"{tripName} was not found";
    }
}