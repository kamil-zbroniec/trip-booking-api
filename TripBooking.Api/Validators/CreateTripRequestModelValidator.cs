namespace TripBooking.Api.Validators;

using Domain.Validations;
using Endpoints.Trips;
using FluentValidation;

public class CreateTripRequestModelValidator : AbstractValidator<CreateTripRequestModel>
{
    public CreateTripRequestModelValidator()
    {
        RuleFor(trip => trip.Name).SetValidator(new NameValidator());
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}