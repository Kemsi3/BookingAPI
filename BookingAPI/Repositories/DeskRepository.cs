using BookingAPI.IServices;
using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Services
{
    public class DeskRepository : IDeskRepository
    {
        public readonly DataContext _context;

        public DeskRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IResult> AddDesk(Desk desk)
        {
            if (desk == null || _context.Desks.Any(d => d.DeskId == desk.DeskId))
                return Results.BadRequest();

            _context.Desks.Add(desk);
            await _context.SaveChangesAsync();
            return Results.Ok(desk);
        }

        public async Task<IResult> DeleteDeskById(string deskId)
        {
            var requestedDesk = _context.Locations.SingleOrDefault(l => l.LocationId == deskId);

            if (requestedDesk == null || _context.Bookings.Any(b => b.DeskId == deskId && b.EndDate > DateTime.Now))
                return Results.BadRequest();

            _context.Locations.Remove(requestedDesk);
            await _context.SaveChangesAsync();
            return Results.Ok();
        }

        public async Task<IResult> GetAllDesks()
        {
            return Results.Ok(await _context.Desks.ToListAsync());
        }

        public async Task<IResult> GetDeskByOfficeId(string locationId)
        {
            return Results.Ok(await _context.Desks.Where(x => x.LocationId == locationId).ToListAsync());
        }
    }
}
