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

public class RegisterForTripRequestHandlerTests
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
            .ReturnsAsync(new TripEntity { NumberOfSeats = 3 });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistration>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    
        var sut = new RegisterForTripRequestHandler(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistration();
        
        var request = new RegisterForTripRequest("name", model); 
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
    
        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        result.AsT0.Should().BeOfType<TripRegistration>();
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
            .ReturnsAsync(new TripEntity { NumberOfSeats = 3 });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistration>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new []{new ValidationFailure()}));
    
        var sut = new RegisterForTripRequestHandler(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistration();
        
        var request = new RegisterForTripRequest("name", model); 
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
    
        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<ValidationFailed>();
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
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistration>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    
        var sut = new RegisterForTripRequestHandler(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistration();
        
        var request = new RegisterForTripRequest("name", model); 
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
    
        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<TripNotFound>();
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
            .ReturnsAsync(new TripEntity { NumberOfSeats = 3 });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistration>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var sut = new RegisterForTripRequestHandler(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistration();

        var request = new RegisterForTripRequest("name", model); 

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<UserAlreadyRegisteredForTrip>();
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
            .ReturnsAsync(new TripEntity
            {
                NumberOfSeats = 3,
                Registrations =
                    [new TripRegistrationEntity(), new TripRegistrationEntity(), new TripRegistrationEntity()]
            });
        
        var createTripRegistrationValidator = new Mock<IValidator<CreateTripRegistration>>();
        createTripRegistrationValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateTripRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    
        var sut = new RegisterForTripRequestHandler(tripRegistrationRepository.Object, tripRepository.Object, createTripRegistrationValidator.Object);

        var model = new CreateTripRegistration();
        
        var request = new RegisterForTripRequest("name", model); 
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
    
        // assert
        tripRegistrationRepository.Verify(x => x.Create(It.IsAny<TripRegistrationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        result.AsT1.Should().BeOfType<TripRegistrationsCountExceeded>();
    }
}