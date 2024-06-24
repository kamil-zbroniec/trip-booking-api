namespace TripBooking.ApplicationServices.Requests;

using Domain.Entities;
using Domain.Repositories;
using MediatR;
using OneOf;
using Contracts;
using Errors;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

public record RegisterForTripRequest(string Name, CreateTripRegistration Model) : IRequest<OneOf<TripRegistration, IOperationError>>;

public class RegisterForTripRequestHandler : IRequestHandler<RegisterForTripRequest, OneOf<TripRegistration, IOperationError>>
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
    
    public async Task<OneOf<TripRegistration, IOperationError>> Handle(RegisterForTripRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createTripRegistrationValidator.ValidateAsync(request.Model, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationException = new ValidationException(validationResult.Errors);
            return new ValidationFailed(validationException.Message); 
        }
    
        var trip = await _tripRepository.Get(request.Name, cancellationToken);
        if (trip is null)
        {
            return new TripNotFound(request.Name);
        }
    
        var alreadyRegistered = await _tripRegistrationRepository.Exists(request.Name, request.Model.UserEmail, cancellationToken);
        if (alreadyRegistered)
        {
            return new UserAlreadyRegisteredForTrip(request.Name, request.Model.UserEmail);
        }
        
        if (trip.Registrations.Count >= trip.NumberOfSeats)
        {
            return new TripRegistrationsCountExceeded(request.Name);            
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