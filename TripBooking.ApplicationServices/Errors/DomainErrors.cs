namespace TripBooking.ApplicationServices.Errors;

using Shared.Results;

public static class DomainErrors
{
    public static class General
    {
        public const string ValidationFailedType = $"{nameof(General)}.{nameof(ValidationFailed)}";
        
        public static Error ValidationFailed(string message) => new Error(
            ValidationFailedType,
            message);
    }
    
    public static class Trip
    {
        public static readonly Error NotFound = new Error(
            $"{nameof(Trip)}.{nameof(NotFound)}",
            "Trip was not found");
        
        public static readonly Error AlreadyExists = new Error(
            $"{nameof(Trip)}.{nameof(AlreadyExists)}",
            "Trip already exists");
    }

    public static class TripRegistration
    {
        public static readonly Error CountExceeded = new Error(
            $"{nameof(TripRegistration)}.{nameof(CountExceeded)}",
            "Trip has already reached its maximum registrations count");
        
        public static readonly Error UserAlreadyRegistered = new Error(
            $"{nameof(TripRegistration)}.{nameof(UserAlreadyRegistered)}",
            "User has already registered for a trip");
    }
}