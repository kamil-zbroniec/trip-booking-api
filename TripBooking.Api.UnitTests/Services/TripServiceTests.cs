namespace TripBooking.Api.UnitTests.Services;

using Api.Services.Trips;
using Domain.Entities;
using Domain.Repositories;
using Dtos;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

public class TripServiceTests
{
    [Fact]
    public async Task Create_WhenValidModel_ReturnsSuccess()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var createTripValidator = new Mock<IValidator<CreateTripDto>>();
        createTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        var updateTripValidator = new Mock<IValidator<UpdateTripDto>>();

        var sut = new TripService(repository.Object, createTripValidator.Object, updateTripValidator.Object);

        var model = new CreateTripDto();

        // act
        var result = await sut.Create(model, CancellationToken.None);

        // assert
        repository.Verify(x => x.Create(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        result.IsSuccess.Should().Be(true);
        result.IsFaulted.Should().Be(false);
    }
    
    [Fact]
    public async Task Create_WhenInvalidModel_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var createTripValidator = new Mock<IValidator<CreateTripDto>>();
        createTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new [] { new ValidationFailure()}));
        
        var updateTripValidator = new Mock<IValidator<UpdateTripDto>>();

        var sut = new TripService(repository.Object, createTripValidator.Object, updateTripValidator.Object);

        var model = new CreateTripDto();

        // act
        var result = await sut.Create(model, CancellationToken.None);

        // assert
        repository.Verify(x => x.Create(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
    
    [Fact]
    public async Task Create_WhenTripExists_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var createTripValidator = new Mock<IValidator<CreateTripDto>>();
        createTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        var updateTripValidator = new Mock<IValidator<UpdateTripDto>>();

        var sut = new TripService(repository.Object, createTripValidator.Object, updateTripValidator.Object);

        var model = new CreateTripDto();

        // act
        var result = await sut.Create(model, CancellationToken.None);

        // assert
        repository.Verify(x => x.Create(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
    
    [Fact]
    public async Task Update_WhenValidModel_ReturnsSuccess()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity());
        
        var createTripValidator = new Mock<IValidator<CreateTripDto>>();
        
        var updateTripValidator = new Mock<IValidator<UpdateTripDto>>();
        updateTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<UpdateTripDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new TripService(repository.Object, createTripValidator.Object, updateTripValidator.Object);

        var model = new UpdateTripDto();

        // act
        var result = await sut.Update("name", model, CancellationToken.None);

        // assert
        repository.Verify(x => x.Update(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        result.IsSuccess.Should().Be(true);
        result.IsFaulted.Should().Be(false);
    }
    
    [Fact]
    public async Task Update_WhenInvalidModel_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity());
        
        var createTripValidator = new Mock<IValidator<CreateTripDto>>();
        
        var updateTripValidator = new Mock<IValidator<UpdateTripDto>>();
        updateTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<UpdateTripDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new [] { new ValidationFailure()}));

        var sut = new TripService(repository.Object, createTripValidator.Object, updateTripValidator.Object);

        var model = new UpdateTripDto();

        // act
        var result = await sut.Update("name", model, CancellationToken.None);

        // assert
        repository.Verify(x => x.Update(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
    
    [Fact]
    public async Task Update_WhenTripNotFound_ReturnsFaulted()
    {
        // arrange
        var repository = new Mock<ITripRepository>();
        repository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TripEntity?)null);
        
        var createTripValidator = new Mock<IValidator<CreateTripDto>>();
        
        var updateTripValidator = new Mock<IValidator<UpdateTripDto>>();
        updateTripValidator
            .Setup(x => x.ValidateAsync(It.IsAny<UpdateTripDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new TripService(repository.Object, createTripValidator.Object, updateTripValidator.Object);

        var model = new UpdateTripDto();

        // act
        var result = await sut.Update("name", model, CancellationToken.None);

        // assert
        repository.Verify(x => x.Update(It.IsAny<TripEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
}