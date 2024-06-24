namespace TripBooking.ApplicationServices.UnitTests.Validators;

using ApplicationServices.Validators;
using Contracts;
using FluentValidation.TestHelper;

public class CreateTripRegistrationValidatorTests
{
    private readonly CreateTripRegistrationValidator _sut = new();

    [Fact]
    public async Task CreateTripRegistratioValidator_WhenEmailIsValid_ShouldNotReturnError()
    {
        // arrange
        var model = new CreateTripRegistration
        {
            UserEmail = "email@mail.com"
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateTripRegistratioValidator_WhenEmailIsInvalid_ShouldReturnError()
    {
        // arrange
        var model = new CreateTripRegistration
        {
            UserEmail = "email@mail@com"
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }
}