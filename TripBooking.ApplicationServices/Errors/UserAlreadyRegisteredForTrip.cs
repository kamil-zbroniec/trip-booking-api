namespace TripBooking.ApplicationServices.Errors;

public struct UserAlreadyRegisteredForTrip : IOperationError
{
    public string Message { get; }

    public UserAlreadyRegisteredForTrip(string tripName, string userEmail)
    {
        Message = $"{userEmail} already registered for trip {tripName}";
    }
}