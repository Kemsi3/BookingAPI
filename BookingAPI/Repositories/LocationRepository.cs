using BookingAPI.IServices;
using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Services
{
    public class LocationRepository : ILocationRepository
    {
        public readonly DataContext _context;

        public LocationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IResult> AddLocation(Location location)
        {
            if (location == null || _context.Locations.Any(l => l.LocationId == location.LocationId))
                return Results.BadRequest();

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return Results.Ok(location);
        }

        public async Task<IResult> DeleteLocationById(string locationId)
        {
            var requestedLocation = _context.Locations.SingleOrDefault(l => l.LocationId == locationId);

            if (requestedLocation == null || _context.Desks.Any(d => d.LocationId == locationId))
                return Results.BadRequest();

            _context.Locations.Remove(requestedLocation);
            await _context.SaveChangesAsync();
            return Results.Ok();
        }

        public async Task<IResult> GetAllLocations()
        {
            return Results.Ok(await _context.Locations.ToListAsync());
        }
    }
}
