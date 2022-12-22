
using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI
{
    public class BookingRepository : IBookingRepository
    {
        public readonly DataContext _context;

        public BookingRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IResult> AddBooking(Booking booking)
        {
            foreach (Booking b in _context.Bookings.Where(b => b.DeskId == booking.DeskId))
            {
                if (!validateDates(b.StartDate, b.EndDate, booking.StartDate, booking.EndDate))
                    return Results.BadRequest();
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return Results.Created($"/booking/{booking.BookingId}", booking);
        }
        

        public async Task<IResult> GetAllBookings()
        {
            return Results.Ok(await _context.Bookings.Where(b=>b.IsDeleted==false).ToListAsync());
        }

        public async Task<IResult> UpdateBooking(Booking bookingToUpdate,string deskId)
        {

            foreach (Booking b in _context.Bookings.Where(b => b.DeskId == deskId && b.IsDeleted == false))
            {
                if (!validateDates(bookingToUpdate.StartDate, bookingToUpdate.EndDate, b.StartDate, b.EndDate) && DateTime.Now.AddDays(1) < bookingToUpdate.StartDate)
                {
                    return Results.BadRequest();
                }
            }

            bookingToUpdate.DeskId = deskId;
            _context.Bookings.Update(bookingToUpdate);
            await _context.SaveChangesAsync();
            return Results.Ok(bookingToUpdate);
        }

        public bool validateDates(DateTime reservedDateStart, DateTime reservedDateEnd, DateTime requestedDateStart, DateTime requestedDateEnd)
        {
            return ((reservedDateEnd < requestedDateStart || (reservedDateStart > requestedDateEnd)) && requestedDateStart < requestedDateEnd);
        }

    }
}
