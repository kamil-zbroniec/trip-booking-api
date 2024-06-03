namespace TripBooking.Api.UnitTests.Validators;

using FluentValidation.TestHelper;
using TripBooking.Api.Validators;

public class NumberOfSeatsValidatorTests
{
    private readonly NumberOfSeatsValidator _sut = new();

    [Fact]
    public async Task NumberOfSeatsValidator_WhenNumberOfSeatsIsTooLow_ShouldReturnError()
    {
        // arrange
        int numberOfSeats = 0;

        // act
        var result = await _sut.TestValidateAsync(numberOfSeats);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task NumberOfSeatsValidator_WhenNumberOfSeatsIsTooBig_ShouldReturnError()
    {
        // arrange
        int numberOfSeats = 101;

        // act
        var result = await _sut.TestValidateAsync(numberOfSeats);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
}