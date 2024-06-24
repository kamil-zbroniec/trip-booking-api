namespace TripBooking.ApplicationServices.Requests;

using Domain.Repositories;
using MediatR;
using Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public record GetTripsByCountryRequest(string Country) : IRequest<IReadOnlyCollection<Trip>>;

public class GetTripsByCountryRequestHandler : IRequestHandler<GetTripsByCountryRequest, IReadOnlyCollection<Trip>>
{
    private readonly ITripRepository _tripRepository;
    
    public GetTripsByCountryRequestHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<IReadOnlyCollection<Trip>> Handle(GetTripsByCountryRequest request, CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.GetByCountry(request.Country, cancellationToken);
        return trips
            .Select(x => x.ToDto())
            .ToArray();
    }
}