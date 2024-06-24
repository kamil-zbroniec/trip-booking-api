namespace TripBooking.ApplicationServices.UnitTests.Validators;

using ApplicationServices.Validators;
using Contracts;
using FluentValidation.TestHelper;

public class CreateTripValidatorTests
{
    private readonly CreateTripValidator _sut = new();

    [Fact]
    public async Task CreateTripValidator_WhenModelIsValid_ShouldNotReturnError()
    {
        // arrange
        var model = new CreateTrip
        {
            Name = "name",
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
    public async Task CreateTripValidator_WhenModelIsInvalid_ShouldReturnError()
    {
        // arrange
        var model = new CreateTrip
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