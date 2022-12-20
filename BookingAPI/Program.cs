

using BookingAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();



app.MapGet("/desks/", async (DataContext context) => await context.Desks.ToListAsync());

app.MapGet("/desks/{locationId}", async (DataContext context, string locationId) => await context.Desks.Where(x => x.LocationId == locationId).ToListAsync());

app.MapGet("desks/getAvailable", async (DataContext context) => await context.Desks.Where(x => x.IsAvailable == true).ToListAsync());

app.MapGet("/desks/getAvailable/{locationId}", async (DataContext context, string locationId) => await context.Desks.Where(x => x.LocationId == locationId && x.IsAvailable == true).ToListAsync());


app.Run();
