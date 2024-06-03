namespace TripBooking.Api.Exceptions;

using System;

public class TripAlreadyExistsException(string message) : Exception(message)
{
    public static TripAlreadyExistsException New(string tripName) 
        => new TripAlreadyExistsException($"{tripName} already exists");
}