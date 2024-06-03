namespace TripBooking.Api.Validators;

using Endpoints.TripRegistrations;
using FluentValidation;
using FluentValidation.Validators;

public class CreateTripRegistrationRequestValidator : AbstractValidator<CreateTripRegistrationRequest>
{
    public CreateTripRegistrationRequestValidator()
    {
        RuleFor(tripRegistration => tripRegistration.UserEmail).EmailAddress(EmailValidationMode.Net4xRegex);
    }
}