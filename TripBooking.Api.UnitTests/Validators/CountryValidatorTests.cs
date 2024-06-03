namespace TripBooking.Api.UnitTests.Validators;

using FluentValidation.TestHelper;
using TripBooking.Api.Validators;

public class CountryValidatorTests
{
    private readonly CountryValidator _sut = new();

    [Fact]
    public async Task CountryValidator_WhenCountryIsEmpty_ShouldReturnError()
    {
        // arrange
        string country = string.Empty;

        // act
        var result = await _sut.TestValidateAsync(country);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task CountryValidator_WhenCountryIsTooLong_ShouldReturnError()
    {
        // arrange
        string country = "too_long_country_1111";

        // act
        var result = await _sut.TestValidateAsync(country);

        // assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
}