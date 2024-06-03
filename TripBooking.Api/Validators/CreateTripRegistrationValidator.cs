namespace TripBooking.Api.Validators;

using Dtos;
using FluentValidation;
using FluentValidation.Validators;

public class CreateTripRegistrationValidator : AbstractValidator<CreateTripRegistrationDto>
{
    public CreateTripRegistrationValidator()
    {
        RuleFor(tripRegistration => tripRegistration.UserEmail).EmailAddress(EmailValidationMode.Net4xRegex);
    }
}