namespace TripBooking.Api.UnitTests.Validators;

using Endpoints.TripRegistrations;
using FluentValidation.TestHelper;
using TripBooking.Api.Validators;

public class CreateTripRegistrationRequestModelValidatorTests
{
    private readonly CreateTripRegistrationRequestModelValidator _sut = new();

    [Theory]
    [InlineData("email@mail..com")]
    [InlineData("email@mail@com")]
    public async Task CreateTripRegistrationRequestValidator_WhenEmailIsInvalid_ShouldReturnError(string email)
    {
        // arrange
        var model = new CreateTripRegistrationRequestModel
        {
            UserEmail = email
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }
}