namespace TripBooking.Api.Services.TripRegistrations;

using Dtos;
using LanguageExt.Common;
using System.Threading;
using System.Threading.Tasks;

public interface ITripRegistrationService
{
    Task<TripRegistrationDto?> Get(string name, string userEmail, CancellationToken cancellationToken);
    
    Task<Result<TripRegistrationDto>> Register(string name, CreateTripRegistrationDto model, CancellationToken cancellationToken);
}