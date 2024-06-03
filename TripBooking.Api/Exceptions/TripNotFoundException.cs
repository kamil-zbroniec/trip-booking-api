namespace TripBooking.Api.Exceptions;

using System;

public class TripNotFoundException(string message) : Exception(message)
{
    public static TripNotFoundException New(string tripName) 
        => new TripNotFoundException($"{tripName} was not found");
}