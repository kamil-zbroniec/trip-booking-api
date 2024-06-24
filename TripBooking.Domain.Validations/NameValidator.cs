namespace TripBooking.Domain.Validations;

using FluentValidation;

public class NameValidator : AbstractValidator<string>
{
    public NameValidator()
    {
        RuleFor(name => name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must be less than 50 characters.")
            .Must(NotContainNewLines).WithMessage("Name cannot contain newline characters.");    
    }
    
    private bool NotContainNewLines(string name)
    {
        return !name.Contains('\n') && 
               !name.Contains('\r');
    }
}