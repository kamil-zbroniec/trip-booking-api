namespace TripBooking.Domain.Repositories;

using Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface ITripRepository
{
    Task<IReadOnlyCollection<TripEntity>> Get(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TripEntity>> GetByCountry(string country, CancellationToken cancellationToken);
    
    Task<TripEntity?> Get(string name, CancellationToken cancellationToken);
    
    Task<bool> Exists(string name, CancellationToken cancellationToken);
    
    Task Create(TripEntity model, CancellationToken cancellationToken);
    
    Task Update(TripEntity entity, CancellationToken cancellationToken);
    
    Task Delete(TripEntity entity, CancellationToken cancellationToken);
}