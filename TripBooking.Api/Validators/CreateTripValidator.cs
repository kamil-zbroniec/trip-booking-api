namespace TripBooking.Api.Validators;

using Dtos;
using FluentValidation;

public class CreateTripValidator : AbstractValidator<CreateTripDto>
{
    public CreateTripValidator()
    {
        RuleFor(trip => trip.Name).SetValidator(new NameValidator());
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}