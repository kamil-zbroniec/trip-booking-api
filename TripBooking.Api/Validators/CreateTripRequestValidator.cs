namespace TripBooking.Api.Validators;

using Endpoints.Trips;
using FluentValidation;

public class CreateTripRequestValidator : AbstractValidator<CreateTripRequest>
{
    public CreateTripRequestValidator()
    {
        RuleFor(trip => trip.Name).SetValidator(new NameValidator());
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}