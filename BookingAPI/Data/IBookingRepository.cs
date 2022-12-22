﻿using BookingAPI.Models;

namespace BookingAPI
{
    public interface IBookingRepository
    {
         Task<IResult> GetAllBookings();

         Task<IResult> UpdateBooking(Booking booking, string deskId);

         Task<IResult> AddBooking(Booking booking);
    }
}
