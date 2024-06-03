namespace TripBooking.Api.Validators;

using Endpoints.Trips;
using FluentValidation;

public class UpdateTripRequestValidator : AbstractValidator<UpdateTripRequest>
{
    public UpdateTripRequestValidator()
    { 
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}