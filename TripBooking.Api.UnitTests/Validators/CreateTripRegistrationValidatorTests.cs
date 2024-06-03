namespace TripBooking.Api.UnitTests.Validators;

using Dtos;
using FluentValidation.TestHelper;
using TripBooking.Api.Validators;

public class CreateTripRegistrationValidatorTests
{
    private readonly CreateTripRegistrationValidator _sut = new();

    [Theory]
    [InlineData("email@mail..com")]
    [InlineData("email@mail@com")]
    public async Task CreateTripRegistratioValidator_WhenEmailIsInvalid_ShouldReturnError(string email)
    {
        // arrange
        var model = new CreateTripRegistrationDto
        {
            UserEmail = email
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }
}