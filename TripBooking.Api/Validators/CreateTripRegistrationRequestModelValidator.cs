namespace TripBooking.Api.Validators;

using Endpoints.TripRegistrations;
using FluentValidation;
using FluentValidation.Validators;

public class CreateTripRegistrationRequestModelValidator : AbstractValidator<CreateTripRegistrationRequestModel>
{
    public CreateTripRegistrationRequestModelValidator()
    {
        RuleFor(tripRegistration => tripRegistration.UserEmail).EmailAddress(EmailValidationMode.Net4xRegex);
    }
}