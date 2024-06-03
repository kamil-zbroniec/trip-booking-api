namespace TripBooking.Api.UnitTests.Validators;

using Api.Validators;
using Endpoints.Trips;
using FluentValidation.TestHelper;

public class UpdateTripRequestValidatorTests
{
    private readonly UpdateTripRequestValidator _sut = new();

    [Fact]
    public async Task UpdateTripRequestValidator_WhenModelIsInvalid_ShouldReturnError()
    {
        // arrange
        var model = new UpdateTripRequest
        {
            Country = string.Empty,
            Description = "description",
            Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
            NumberOfSeats = 0
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldHaveValidationErrorFor(x => x.Country);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfSeats);
    }
}