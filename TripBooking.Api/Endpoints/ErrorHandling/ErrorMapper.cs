namespace TripBooking.Api.Endpoints.ErrorHandling;

using System;

public static class ErrorMapper
{
    public static ErrorResponse ToResponse(this Exception source) =>
        new()
        {
            Message = source.Message
        };
}