namespace TripBooking.ApplicationServices.Errors;

public struct TripAlreadyExists : IOperationError
{
    public string Message { get; }

    public TripAlreadyExists(string tripName)
    {
        Message = $"{tripName} already exists";
    }
}