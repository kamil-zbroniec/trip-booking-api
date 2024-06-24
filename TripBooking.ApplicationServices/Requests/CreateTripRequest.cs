namespace TripBooking.ApplicationServices.Requests;

using Contracts;
using Domain.Entities;
using Domain.Repositories;
using Errors;
using FluentValidation;
using MediatR;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

public record CreateTripRequest(CreateTrip Model) : IRequest<OneOf<Trip, IOperationError>>;

public class CreateTripRequestHandler : IRequestHandler<CreateTripRequest, OneOf<Trip, IOperationError>>
{
    private readonly ITripRepository _tripRepository;
    private readonly IValidator<CreateTrip> _createTripValidator;
    
    public CreateTripRequestHandler(
        ITripRepository tripRepository, 
        IValidator<CreateTrip> createTripValidator)
    {
        _tripRepository = tripRepository;
        _createTripValidator = createTripValidator;
    }

    public async Task<OneOf<Trip, IOperationError>> Handle(CreateTripRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createTripValidator.ValidateAsync(request.Model, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return new ValidationFailed(validationException.Message);
        }

        var exists = await _tripRepository.Exists(request.Model.Name, cancellationToken);
        
        if (exists)
        {
            return new TripAlreadyExists(request.Model.Name);
        }

        var trip = new TripEntity
        {
            Name = request.Model.Name,
            Country = request.Model.Country,
            Description = request.Model.Description,
            Start = request.Model.Start,
            NumberOfSeats = request.Model.NumberOfSeats,
        };

        await _tripRepository.Create(trip, cancellationToken);
        
        return trip.ToDto();
    }
}