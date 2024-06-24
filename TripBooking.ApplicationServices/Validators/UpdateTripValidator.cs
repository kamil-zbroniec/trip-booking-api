namespace TripBooking.ApplicationServices.Validators;

using Contracts;
using Domain.Validations;
using FluentValidation;

public class UpdateTripValidator : AbstractValidator<UpdateTrip>
{
    public UpdateTripValidator()
    { 
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}