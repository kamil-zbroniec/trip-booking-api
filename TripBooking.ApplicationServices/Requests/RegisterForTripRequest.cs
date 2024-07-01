namespace TripBooking.ApplicationServices.Requests;

using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Contracts;
using Errors;
using FluentValidation;
using Shared.Results;
using System.Threading;
using System.Threading.Tasks;

public record RegisterForTripRequest(string Name, CreateTripRegistration Model) : IRequest<Result<TripRegistration>>;

public class RegisterForTripRequestHandler : IRequestHandler<RegisterForTripRequest, Result<TripRegistration>>
{
    private readonly ITripRegistrationRepository _tripRegistrationRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IValidator<CreateTripRegistration> _createTripRegistrationValidator;

    public RegisterForTripRequestHandler(
        ITripRegistrationRepository tripRegistrationRepository,
        ITripRepository tripRepository,
        IValidator<CreateTripRegistration> createTripRegistrationValidator)
    {
        _tripRegistrationRepository = tripRegistrationRepository;
        _tripRepository = tripRepository;
        _createTripRegistrationValidator = createTripRegistrationValidator;
    }
    
    public async Task<Result<TripRegistration>> Handle(RegisterForTripRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createTripRegistrationValidator.ValidateAsync(request.Model, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return Result<TripRegistration>.Failed(DomainErrors.General.ValidationFailed(validationException.Message)); 
        }
    
        var trip = await _tripRepository.Get(request.Name, cancellationToken);
        if (trip is null)
        {
            return Result<TripRegistration>.Failed(DomainErrors.Trip.NotFound);
        }
    
        var alreadyRegistered = await _tripRegistrationRepository.Exists(request.Name, request.Model.UserEmail, cancellationToken);
        if (alreadyRegistered)
        {
            return Result<TripRegistration>.Failed(DomainErrors.TripRegistration.UserAlreadyRegistered);
        }
        
        if (trip.Registrations.Count >= trip.NumberOfSeats)
        {
            return Result<TripRegistration>.Failed(DomainErrors.TripRegistration.CountExceeded);
        }
        
        var tripRegistration = new TripRegistrationEntity
        {
            TripName = request.Name,
            UserEmail = request.Model.UserEmail
        };
    
        await _tripRegistrationRepository.Create(tripRegistration, cancellationToken);
    
        return tripRegistration.ToDto();
    }
}