namespace TripBooking.Api.UnitTests.Validators;

using Api.Validators;
using Endpoints.Trips;
using FluentValidation.TestHelper;

public class CreateTripRequestModelValidatorTests
{
    private readonly CreateTripRequestModelValidator _sut = new();

    [Fact]
    public async Task CreateTripRequestValidator_WhenModelIsInvalid_ShouldReturnError()
    {
        // arrange
        var model = new CreateTripRequestModel
        {
            Name = string.Empty,
            Country = string.Empty,
            Description = "description",
            Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
            NumberOfSeats = 0
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Country);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfSeats);
    }
}