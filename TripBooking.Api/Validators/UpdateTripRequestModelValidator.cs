namespace TripBooking.Api.Validators;

using Domain.Validations;
using Endpoints.Trips;
using FluentValidation;

public class UpdateTripRequestModelValidator : AbstractValidator<UpdateTripRequestModel>
{
    public UpdateTripRequestModelValidator()
    { 
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}