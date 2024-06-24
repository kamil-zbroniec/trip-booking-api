namespace TripBooking.ApplicationServices.Requests;

using Contracts;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public record GetTripRegistrationRequest(string Name, string UserEmail) : IRequest<TripRegistration?>;

public class GetTripRegistrationRequestHandler : IRequestHandler<GetTripRegistrationRequest, TripRegistration?>
{
    private readonly ITripRegistrationRepository _tripRegistrationRepository;

    public GetTripRegistrationRequestHandler(ITripRegistrationRepository tripRegistrationRepository)
    {
        _tripRegistrationRepository = tripRegistrationRepository;
    }

    public async Task<TripRegistration?> Handle(GetTripRegistrationRequest request, CancellationToken cancellationToken)
    {
        var tripRegistration = await _tripRegistrationRepository.Get(request.Name, request.UserEmail, cancellationToken);
        return tripRegistration?.ToDto();
    }
}

