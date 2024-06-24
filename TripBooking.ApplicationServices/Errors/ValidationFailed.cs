namespace TripBooking.ApplicationServices.Errors;

public struct ValidationFailed : IOperationError
{
    public string Message { get; }

    public ValidationFailed(string message)
    {
        Message = message;
    }
}