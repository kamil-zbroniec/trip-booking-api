namespace TripBooking.Domain.Validations;

using FluentValidation;
using FluentValidation.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(email => email)
            .NotEmpty()
            .EmailAddress(EmailValidationMode.Net4xRegex);    
    }
}