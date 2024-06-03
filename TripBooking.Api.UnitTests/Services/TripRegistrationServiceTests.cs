namespace TripBooking.Api.UnitTests.Services;

using Api.Services.TripRegistrations;
using Domain.Entities;
using Domain.Repositories;
using Dtos;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

public class TripRegistrationServiceTests
{
    [Fact]
    public async Task Register_WhenValidModel_ReturnsSuccess()
    {
        // arrange
        var tripRegistrationRepository = new Mock<ITripRegistrationRepository>();
        tripRegistrationRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var tripRepository = new Mock<ITripRepository>();
        tripRepository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity() { NumberOfSeats = 3 });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistrationDto>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistrationDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new TripRegistrationService(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistrationDto();

        // act
        var result = await sut.Register("name", model, CancellationToken.None);

        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        result.IsSuccess.Should().Be(true);
        result.IsFaulted.Should().Be(false);
    }
    
    [Fact]
    public async Task Register_WhenInvalidModel_ReturnsFaulted()
    {
        // arrange
        var tripRegistrationRepository = new Mock<ITripRegistrationRepository>();
        tripRegistrationRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var tripRepository = new Mock<ITripRepository>();
        tripRepository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity() { NumberOfSeats = 3 });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistrationDto>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistrationDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new []{new ValidationFailure()}));

        var sut = new TripRegistrationService(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistrationDto();

        // act
        var result = await sut.Register("name", model, CancellationToken.None);

        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
    
    [Fact]
    public async Task Register_WhenTripNotExists_ReturnsFaulted()
    {
        // arrange
        var tripRegistrationRepository = new Mock<ITripRegistrationRepository>();
        tripRegistrationRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var tripRepository = new Mock<ITripRepository>();
        tripRepository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TripEntity?)null);
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistrationDto>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistrationDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new TripRegistrationService(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistrationDto();

        // act
        var result = await sut.Register("name", model, CancellationToken.None);

        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
    
    [Fact]
    public async Task Register_WhenAlreadyRegisteredForTrip_ReturnsFaulted()
    {
        // arrange
        var tripRegistrationRepository = new Mock<ITripRegistrationRepository>();
        tripRegistrationRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var tripRepository = new Mock<ITripRepository>();
        tripRepository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity() { NumberOfSeats = 3 });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistrationDto>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistrationDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new TripRegistrationService(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistrationDto();

        // act
        var result = await sut.Register("name", model, CancellationToken.None);

        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
    
    [Fact]
    public async Task Register_WhenAllSeatsAlreadyTaken_ReturnsFaulted()
    {
        // arrange
        var tripRegistrationRepository = new Mock<ITripRegistrationRepository>();
        tripRegistrationRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var tripRepository = new Mock<ITripRepository>();
        tripRepository
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TripEntity()
            {
                NumberOfSeats = 3,
                Registrations =
                    [new TripRegistrationEntity(), new TripRegistrationEntity(), new TripRegistrationEntity()]
            });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistrationDto>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistrationDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new TripRegistrationService(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistrationDto();

        // act
        var result = await sut.Register("name", model, CancellationToken.None);

        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.IsSuccess.Should().Be(false);
        result.IsFaulted.Should().Be(true);
    }
}