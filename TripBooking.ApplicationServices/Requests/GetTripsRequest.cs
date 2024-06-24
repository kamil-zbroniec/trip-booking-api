namespace TripBooking.ApplicationServices.Requests;

using Domain.Repositories;
using MediatR;
using Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// NOTE - It would be a good practice to implement pagination for methods that may cause retrieval of large amounts
// of data. This simple approach should be limited to the databases that contain only minimal amounts of data.
public record GetTripsRequest() : IRequest<IReadOnlyCollection<Trip>>;

public class GetTripsRequestHandler : IRequestHandler<GetTripsRequest, IReadOnlyCollection<Trip>>
{
    private readonly ITripRepository _tripRepository;
    
    public GetTripsRequestHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<IReadOnlyCollection<Trip>> Handle(GetTripsRequest request, CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.Get(cancellationToken);
        return trips.Select(x => x.ToDto())
            .ToArray();
    }
}