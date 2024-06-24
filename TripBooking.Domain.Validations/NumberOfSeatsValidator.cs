namespace TripBooking.Domain.Validations;

using FluentValidation;

public class NumberOfSeatsValidator : AbstractValidator<int>
{
    public NumberOfSeatsValidator()
    {
        RuleFor(numberOfSeats => numberOfSeats)
            .InclusiveBetween(1, 100).WithMessage("Number of seats must be between 1 and 100");
    }
}