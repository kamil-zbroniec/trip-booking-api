namespace TripBooking.Api.Services.Trips;

using Dtos;
using LanguageExt.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface ITripService
{
    Task<IReadOnlyCollection<TripDto>> Get(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TripDto>> GetByCountry(string country, CancellationToken cancellationToken);

    Task<TripDto?> Get(string name, CancellationToken cancellationToken);

    Task<Result<TripDto>> Create(CreateTripDto model, CancellationToken cancellationToken);

    Task<Result<TripDto>> Update(string name, UpdateTripDto model, CancellationToken cancellationToken);
    
    Task Delete(string name, CancellationToken cancellationToken);
}