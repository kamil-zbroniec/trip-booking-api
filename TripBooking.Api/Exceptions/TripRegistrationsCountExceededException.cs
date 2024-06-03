namespace TripBooking.Api.Exceptions;

using System;

public class TripRegistrationsCountExceededException(string message) : Exception(message)
{
    public static TripRegistrationsCountExceededException New(string tripName) 
        => new TripRegistrationsCountExceededException($"Trip {tripName} has already reached its maximum registrations count");
}