namespace TripBooking.Api.Exceptions;

using System;

public class UserAlreadyRegisteredForTripException(string message) : Exception(message)
{
    public static UserAlreadyRegisteredForTripException New(string tripName, string userEmail) 
        => new UserAlreadyRegisteredForTripException($"{userEmail} already registered for trip {tripName}");
}