namespace TripBooking.Infrastructure.Repositories;

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class TripRepository : ITripRepository
{
    private readonly TripBookingDbContext _context;

    public TripRepository(TripBookingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TripEntity>> Get(CancellationToken cancellationToken)
    {
        return await _context.Trips
            .AsNoTracking()
            .Include(x => x.Registrations)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TripEntity>> GetByCountry(string country, CancellationToken cancellationToken)
    {
        return await _context.Trips
            .AsNoTracking()
            .Include(x => x.Registrations)
            .Where(x => x.Country.ToLower() == country.ToLower())
            .ToArrayAsync(cancellationToken);
    }

    public async Task<TripEntity?> Get(string name, CancellationToken cancellationToken)
    {
        return await _context.Trips
            .AsNoTracking()
            .Include(x => x.Registrations)
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<bool> Exists(string name, CancellationToken cancellationToken)
    {
        return await _context.Trips
            .AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task Create(TripEntity model, CancellationToken cancellationToken)
    {
        _context.Trips.Add(model);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(TripEntity entity, CancellationToken cancellationToken)
    {
        _context.Trips.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(TripEntity entity, CancellationToken cancellationToken)
    {
        _context.Trips.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}