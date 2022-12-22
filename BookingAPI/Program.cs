using BookingAPI;
using BookingAPI.IServices;
using BookingAPI.Models;
using BookingAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/desks/", async (IDeskRepository deskRepository) => await deskRepository.GetAllDesks());

app.MapGet("/desk/{locationId}", async (IDeskRepository deskRepository, string locationId) => await deskRepository.GetDeskByOfficeId(locationId));

app.MapGet("/bookings", async (IBookingRepository bookingRepository) => await bookingRepository.GetAllBookings());

app.MapGet("/locations", async (ILocationRepository locationRepository) => await locationRepository.GetAllLocations());

app.MapPost("/booking/", async (IBookingRepository bookingRepository, Booking booking) => await bookingRepository.AddBooking(booking));

app.MapPost("/location", async (ILocationRepository locationRepository, Location location) => await locationRepository.AddLocation(location));

app.MapPut("/booking/{deskId}", async (IBookingRepository bookingRepository, Booking booking, string deskId) => await bookingRepository.UpdateBooking(booking, deskId));

app.MapDelete("/location/{locationId}", async (ILocationRepository locationRepository, string locationId) => await locationRepository.DeleteLocationById(locationId));

app.MapPost("/desk/", async (IDeskRepository deskRepository, Desk desk) => await deskRepository.AddDesk(desk));

app.MapDelete("/desk/deskId", async (IDeskRepository deskRepository, string deskId) => await deskRepository.DeleteDeskById(deskId));


app.Run();




