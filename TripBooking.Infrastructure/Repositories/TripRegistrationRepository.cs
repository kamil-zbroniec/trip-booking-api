namespace TripBooking.Infrastructure.Repositories;

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class TripRegistrationRepository : ITripRegistrationRepository
{
    private readonly TripBookingDbContext _context;

    public TripRegistrationRepository(TripBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<TripRegistrationEntity?> Get(string name, string email, CancellationToken cancellationToken)
    {
        return await _context.TripRegistrations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TripName.ToLower() == name.ToLower() && 
                                      x.UserEmail.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<bool> Exists(string name, string email, CancellationToken cancellationToken)
    {
        return await _context.TripRegistrations
            .AsNoTracking()
            .AnyAsync(x => x.TripName.ToLower() == name.ToLower() && 
                           x.UserEmail.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task Create(TripRegistrationEntity model, CancellationToken cancellationToken)
    {
        _context.TripRegistrations.Add(model);
        await _context.SaveChangesAsync(cancellationToken);
    }
}