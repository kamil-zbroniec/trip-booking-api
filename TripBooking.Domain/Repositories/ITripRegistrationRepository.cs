namespace TripBooking.Domain.Repositories;

using Entities;
using System.Threading;
using System.Threading.Tasks;

public interface ITripRegistrationRepository
{
    Task<TripRegistrationEntity?> Get(string name, string email, CancellationToken cancellationToken);
    
    Task<bool> Exists(string name, string email, CancellationToken cancellationToken);
    
    Task Create(TripRegistrationEntity model, CancellationToken cancellationToken);
}