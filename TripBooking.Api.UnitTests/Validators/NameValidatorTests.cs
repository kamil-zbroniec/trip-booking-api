namespace TripBooking.Api.UnitTests.Validators;

using FluentValidation.TestHelper;
using TripBooking.Api.Validators;

public class NameValidatorTests
{
    private readonly NameValidator _sut = new();

    [Fact]
    public async Task NameValidator_WhenNameIsEmpty_ShouldReturnError()
    {
        // arrange
        string name = string.Empty;

        // act
        var result = await _sut.TestValidateAsync(name);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task NameValidator_WhenNameIsTooLong_ShouldReturnError()
    {
        // arrange
        string name = "too_long_country_1111_123456789_123456789_1234567890";

        // act
        var result = await _sut.TestValidateAsync(name);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task NameValidator_WhenNameContainsNewLineCharacters_ShouldReturnError()
    {
        // arrange
        string name = "name\nname";

        // act
        var result = await _sut.TestValidateAsync(name);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
}