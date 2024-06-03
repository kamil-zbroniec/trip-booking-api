namespace TripBooking.Api.Services.Trips;

using Domain.Entities;
using Domain.Repositories;
using Dtos;
using Exceptions;
using FluentValidation;
using LanguageExt.Common;
using Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TripDto = Dtos.TripDto;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly IValidator<CreateTripDto> _createTripValidator;
    private readonly IValidator<UpdateTripDto> _updateTripValidator;

    public TripService(
        ITripRepository tripRepository, 
        IValidator<CreateTripDto> createTripValidator,
        IValidator<UpdateTripDto> updateTripValidator)
    {
        _tripRepository = tripRepository;
        _createTripValidator = createTripValidator;
        _updateTripValidator = updateTripValidator;
    }

    // NOTE - It would be a good practice to implement pagination for methods that may cause retrieval of large amounts
    // of data. This simple approach should be limited to the databases that contain only minmal amounts of data.
    public async Task<IReadOnlyCollection<TripDto>> Get(CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.Get(cancellationToken);
        
        return trips.Select(x => x.ToDto())
            .ToArray();
    }

    public async Task<IReadOnlyCollection<TripDto>> GetByCountry(string country, CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.GetByCountry(country, cancellationToken);
        
        return trips
            .Select(x => x.ToDto())
            .ToArray();
    }
    
    public async Task<TripDto?> Get(string name, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.Get(name, cancellationToken);
        
        return trip?.ToDto();
    }
    
    public async Task<Result<TripDto>> Create(CreateTripDto model, CancellationToken cancellationToken)
    {
        var validationResult = await _createTripValidator.ValidateAsync(model, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return new Result<TripDto>(validationException);
        }

        var exists = await _tripRepository.Exists(model.Name, cancellationToken);
        
        if (exists)
        {
            var tripAlreadyExistsException = TripAlreadyExistsException.New(model.Name);
            return new Result<TripDto>(tripAlreadyExistsException);
        }

        var trip = new TripEntity
        {
            Name = model.Name,
            Country = model.Country,
            Description = model.Description,
            Start = model.Start,
            NumberOfSeats = model.NumberOfSeats,
        };

        await _tripRepository.Create(trip, cancellationToken);
        
        return new Result<TripDto>(trip.ToDto());
    }

    public async Task<Result<TripDto>> Update(string name, UpdateTripDto model, CancellationToken cancellationToken)
    {
        var validationResult = await _updateTripValidator.ValidateAsync(model, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return new Result<TripDto>(validationException);
        }

        var trip = await _tripRepository.Get(name, cancellationToken);
    
        if (trip is null)
        {
            var tripNotFound = TripNotFoundException.New(name);
            return new Result<TripDto>(tripNotFound);
        }
    
        trip.Country = model.Country;
        trip.Description = model.Description;
        trip.Start = model.Start;
        trip.NumberOfSeats = model.NumberOfSeats;

        await _tripRepository.Update(trip, cancellationToken);

        return new Result<TripDto>(trip.ToDto());
    }
    
    public async Task Delete(string name, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.Get(name, cancellationToken);
    
        if (trip is null)
        {
            return;
        }

        await _tripRepository.Delete(trip, cancellationToken);
    }
}