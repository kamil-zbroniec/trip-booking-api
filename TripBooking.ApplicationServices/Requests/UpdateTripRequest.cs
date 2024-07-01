namespace TripBooking.ApplicationServices.Requests;

using Contracts;
using Domain.Repositories;
using Errors;
using FluentValidation;
using MediatR;
using Shared.Results;
using System.Threading;
using System.Threading.Tasks;

public record UpdateTripRequest(string Name, UpdateTrip Model) : IRequest<Result<Trip>>;

public class UpdateTripRequestHandler : IRequestHandler<UpdateTripRequest, Result<Trip>>
{
    private readonly ITripRepository _tripRepository;
    private readonly IValidator<UpdateTrip> _updateTripValidator;
    
    public UpdateTripRequestHandler(
        ITripRepository tripRepository,
        IValidator<UpdateTrip> updateTripValidator)
    {
        _tripRepository = tripRepository;
        _updateTripValidator = updateTripValidator;
    }

    public async Task<Result<Trip>> Handle(UpdateTripRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateTripValidator.ValidateAsync(request.Model, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return Result<Trip>.Failed(DomainErrors.General.ValidationFailed(validationException.Message));
        }

        var trip = await _tripRepository.Get(request.Name, cancellationToken);
    
        if (trip is null)
        {
            return Result<Trip>.Failed(DomainErrors.Trip.NotFound);
        }
    
        trip.Country = request.Model.Country;
        trip.Description = request.Model.Description;
        trip.Start = request.Model.Start;
        trip.NumberOfSeats = request.Model.NumberOfSeats;

        await _tripRepository.Update(trip, cancellationToken);

        return trip.ToDto();
    }
}