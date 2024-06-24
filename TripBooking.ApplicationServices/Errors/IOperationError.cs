namespace TripBooking.ApplicationServices.Errors;

public interface IOperationError
{
    string Message { get; }
}