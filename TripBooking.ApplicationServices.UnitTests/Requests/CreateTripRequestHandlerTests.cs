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

public class CreateTripRequestHandlerTests
{
    [Fact]
    public async Task Create_WhenValidModel_ReturnsSuccess()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var createTripValidator = new Mock<IValidator<CreateTrip>>();
        createTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTrip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        var sut = new CreateTripRequestHandler(repository.Object, createTripValidator.Object);

        var model = new CreateTrip();

        var request = new CreateTripRequest(model);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Create(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        result.AsT0.Should().BeOfType<Trip>();
    }
    
    [Fact]
    public async Task Create_WhenInvalidModel_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var createTripValidator = new Mock<IValidator<CreateTrip>>();
        createTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTrip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new [] { new ValidationFailure()}));
        
        var sut = new CreateTripRequestHandler(repository.Object, createTripValidator.Object);

        var model = new CreateTrip();

        var request = new CreateTripRequest(model);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Create(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<ValidationFailed>();
    }
    
    [Fact]
    public async Task Create_WhenTripExists_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var createTripValidator = new Mock<IValidator<CreateTrip>>();
        createTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTrip>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new CreateTripRequestHandler(repository.Object, createTripValidator.Object);

        var model = new CreateTrip();

        var request = new CreateTripRequest(model);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        repository.Verify(x => x.Create(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<TripAlreadyExists>();
    }
}