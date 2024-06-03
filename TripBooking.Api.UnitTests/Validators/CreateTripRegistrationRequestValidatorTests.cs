namespace TripBooking.Api.UnitTests.Validators;

using Endpoints.TripRegistrations;
using FluentValidation.TestHelper;
using TripBooking.Api.Validators;

public class CreateTripRegistrationRequestValidatorTests
{
    private readonly CreateTripRegistrationRequestValidator _sut = new();

    [Theory]
    [InlineData("email@mail..com")]
    [InlineData("email@mail@com")]
    public async Task CreateTripRegistrationRequestValidator_WhenEmailIsInvalid_ShouldReturnError(string email)
    {
        // arrange
        var model = new CreateTripRegistrationRequest
        {
            UserEmail = email
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }
}