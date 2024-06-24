namespace TripBooking.ApplicationServices.Validators;

using Contracts;
using Domain.Validations;
using FluentValidation;

public class CreateTripValidator : AbstractValidator<CreateTrip>
{
    public CreateTripValidator()
    {
        RuleFor(trip => trip.Name).SetValidator(new NameValidator());
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}