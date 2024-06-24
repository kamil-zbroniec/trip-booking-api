namespace TripBooking.ApplicationServices.UnitTests.Requests;

using ApplicationServices.Requests;
using Contracts;
using Domain.Entities;
using Domain.Repositories;
using Errors;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

public class UpdateTripRequestHandlerTests
{
    [Fact]
    public async Task Update_WhenValidModel_ReturnsSuccess()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity());
        
        var updateTripValidator = new Mock<IValidator<UpdateTrip>>();
        updateTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<UpdateTrip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new UpdateTripRequestHandler(repository.Object, updateTripValidator.Object);

        var model = new UpdateTrip();

        var request = new UpdateTripRequest("name", model);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Update(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        result.AsT0.Should().BeOfType<Trip>();
    }
    
    [Fact]
    public async Task Update_WhenInvalidModel_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity());
        
        var updateTripValidator = new Mock<IValidator<UpdateTrip>>();
        updateTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<UpdateTrip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new [] { new ValidationFailure()}));

        var sut = new UpdateTripRequestHandler(repository.Object, updateTripValidator.Object);

        var model = new UpdateTrip();

        var request = new UpdateTripRequest("name", model);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Update(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<ValidationFailed>();
    }
    
    [Fact]
    public async Task Update_WhenTripNotFound_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TripEntity?)null);
        
        var updateTripValidator = new Mock<IValidator<UpdateTrip>>();
        updateTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<UpdateTrip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new UpdateTripRequestHandler(repository.Object, updateTripValidator.Object);

        var model = new UpdateTrip();

        var request = new UpdateTripRequest("name", model);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Update(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<TripNotFound>();
    }
}