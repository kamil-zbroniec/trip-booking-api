namespace TripBooking.ApplicationServices.Requests;

using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public record DeleteTripRequest(string Name) : IRequest;

public class DeleteTripRequestHandler : IRequestHandler<DeleteTripRequest>
{
    private readonly ITripRepository _tripRepository;
    
    public DeleteTripRequestHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task Handle(DeleteTripRequest request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.Get(request.Name, cancellationToken);
        if (trip is null)
        {
            return;
        }

        await _tripRepository.Delete(trip, cancellationToken);
    }
}