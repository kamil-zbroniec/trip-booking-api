namespace TripBooking.ApplicationServices.Requests;

using Domain.Repositories;
using MediatR;
using Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public record GetTripByNameRequest(string Name) : IRequest<Trip?>;

public class GetTripByNameRequestHandler : IRequestHandler<GetTripByNameRequest, Trip?>
{
    private readonly ITripRepository _tripRepository;
    
    public GetTripByNameRequestHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Trip?> Handle(GetTripByNameRequest request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.Get(request.Name, cancellationToken);
        return trip?.ToDto();
    }
}