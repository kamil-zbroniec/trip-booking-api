namespace TripBooking.Domain.Validations;

using FluentValidation;

public class CountryValidator : AbstractValidator<string>
{
    public CountryValidator()
    {
        RuleFor(country => country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(20).WithMessage("Country must be less than 20 characters");    
    }
}