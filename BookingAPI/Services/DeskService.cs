using BookingAPI.IServices;
using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Services
{
    public class DeskService : IDeskService
    {
        public readonly DataContext _context;

        public DeskService(DataContext context)
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

        public async Task<IResult> GetAvailableDesks(DateTime startDate, DateTime endDate)
        {
            List<Desk> availableDesks = new List<Desk>();

            foreach(Desk d in _context.Desks)
            {
                if (!(_context.Bookings.Any(b => b.DeskId == d.DeskId && !(b.EndDate < startDate || b.StartDate > endDate))))
                {
                    availableDesks.Add(d);
                }
            }
            return Results.Ok(availableDesks);
        }

        public async Task<IResult> GetAllDesks()
        {
            return Results.Ok(await _context.Desks.ToListAsync());
        }

        public async Task<IResult> GetDeskByOfficeId(string locationId)
        {
            return Results.Ok(await _context.Desks.Where(x => x.LocationId == locationId).ToListAsync());
        }


        public bool validateDates(DateTime reservedDateStart, DateTime reservedDateEnd, DateTime requestedDateStart, DateTime requestedDateEnd)
        {
            return ((reservedDateEnd < requestedDateStart || (reservedDateStart > requestedDateEnd)) && requestedDateStart < requestedDateEnd);
        }
    }
}
