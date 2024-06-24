namespace TripBooking.ApplicationServices.UnitTests.Validators;

using ApplicationServices.Validators;
using Contracts;
using FluentValidation.TestHelper;

public class UpdateTripValidatorTests
{
    private readonly UpdateTripValidator _sut = new();

    [Fact]
    public async Task UpdateTripValidator_WhenModelIsValid_ShouldNotReturnError()
    {
        // arrange
        var model = new UpdateTrip
        {
            Country = "country",
            Description = "description",
            Start = new DateTime(2024, 06, 03, 00, 00, 00, DateTimeKind.Utc),
            NumberOfSeats = 12
        };

        // act
        var result = await _sut.TestValidateAsync(model);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UpdateTripValidator_WhenModelIsInvalid_ShouldReturnError()
    {
        // arrange
        var model = new UpdateTrip
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