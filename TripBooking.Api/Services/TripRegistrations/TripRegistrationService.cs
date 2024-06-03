namespace TripBooking.Api.Services.TripRegistrations;

using Domain.Entities;
using Domain.Repositories;
using Dtos;
using Exceptions;
using FluentValidation;
using LanguageExt.Common;
using Mappers;
using System.Threading;
using System.Threading.Tasks;

public class TripRegistrationService : ITripRegistrationService
{
    private readonly ITripRegistrationRepository _tripRegistrationRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IValidator<CreateTripRegistrationDto> _createTripRegistrationValidator;

    public TripRegistrationService(ITripRegistrationRepository tripRegistrationRepository, ITripRepository tripRepository, IValidator<CreateTripRegistrationDto> createTripRegistrationValidator)
    {
        _tripRegistrationRepository = tripRegistrationRepository;
        _tripRepository = tripRepository;
        _createTripRegistrationValidator = createTripRegistrationValidator;
    }
    
    public async Task<TripRegistrationDto?> Get(string name, string userEmail, CancellationToken cancellationToken)
    {
        var tripRegistration = await _tripRegistrationRepository.Get(name, userEmail, cancellationToken);
        return tripRegistration?.ToDto();
    }
    
    public async Task<Result<TripRegistrationDto>> Register(string name, CreateTripRegistrationDto model, CancellationToken cancellationToken)
    {
        var validationResult = await _createTripRegistrationValidator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return new Result<TripRegistrationDto>(validationException);
        }

        var trip = await _tripRepository.Get(name, cancellationToken);
        if (trip is null)
        {
            var tripNotFoundException = TripNotFoundException.New(name);
            return new Result<TripRegistrationDto>(tripNotFoundException);
        }

        var alreadyRegistered = await _tripRegistrationRepository.Exists(name, model.UserEmail, cancellationToken);
        if (alreadyRegistered)
        {
            var userAlreadyRegisteredForTripException = UserAlreadyRegisteredForTripException.New(name, model.UserEmail);
            return new Result<TripRegistrationDto>(userAlreadyRegisteredForTripException);
        }
        
        if (trip.Registrations.Count >= trip.NumberOfSeats)
        {
            var tripRegistrationsCountExceededException = TripRegistrationsCountExceededException.New(name);
            return new Result<TripRegistrationDto>(tripRegistrationsCountExceededException);
        }
        
        var tripRegistration = new TripRegistrationEntity
        {
            TripName = name,
            UserEmail = model.UserEmail
        };

        await _tripRegistrationRepository.Create(tripRegistration, cancellationToken);

        return new Result<TripRegistrationDto>(tripRegistration.ToDto());
    }
}