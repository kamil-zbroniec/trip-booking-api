namespace TripBooking.ApplicationServices.Validators;

using Contracts;
using Domain.Validations;
using FluentValidation;

public class CreateTripRegistrationValidator : AbstractValidator<CreateTripRegistration>
{
    public CreateTripRegistrationValidator()
    {
        RuleFor(tripRegistration => tripRegistration.UserEmail).SetValidator(new EmailValidator());
    }
}