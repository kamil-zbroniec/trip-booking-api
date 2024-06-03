namespace TripBooking.Api.Validators;

using Dtos;
using FluentValidation;

public class UpdateTripValidator : AbstractValidator<UpdateTripDto>
{
    public UpdateTripValidator()
    { 
        RuleFor(trip => trip.Country).SetValidator(new CountryValidator());
        RuleFor(trip => trip.NumberOfSeats).SetValidator(new NumberOfSeatsValidator());
    }
}