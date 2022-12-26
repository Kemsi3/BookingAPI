using BookingAPI;
using BookingAPI.IServices;
using BookingAPI.Models;
using BookingAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000"
                                              ); 
                       });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IDeskService, DeskService>();
builder.Services.AddScoped<ILocationProvider, LocationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Description = "Bearer Authentication with JWT Token",
//        Type = SecuritySchemeType.Http
//    });
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Id = "Bearer",
//                    Type = ReferenceType.SecurityScheme
//                }
//            },
//            new List<string>()
//        }
//    });
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateActor = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});
//builder.Services.AddAuthorization();

//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//app.UseSwagger();
//app.UseAuthorization();
//app.UseAuthentication();
app.UseCors(MyAllowSpecificOrigins);

//IResult Login(UserLogin user, IUserService service)
//{
//    if (!string.IsNullOrEmpty(user.Name) &&
//        !string.IsNullOrEmpty(user.Password))
//    {
//        var loggedInUser = service.Get(user);
//        if (loggedInUser is null) return Results.NotFound("User not found");

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.Name, loggedInUser.Name),
//            new Claim(ClaimTypes.Role, loggedInUser.Role)
//        };

//        var token = new JwtSecurityToken
//        (
//            issuer: builder.Configuration["Jwt:Issuer"],
//            audience: builder.Configuration["Jwt:Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddDays(60),
//            notBefore: DateTime.UtcNow,
//            signingCredentials: new SigningCredentials(
//                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//                SecurityAlgorithms.HmacSha256)
//        );

//        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

//        return Results.Ok(tokenString);
//    }
//    return Results.BadRequest("Invalid user credentials");
//}

//app.MapPost("/login",
//(UserLogin user, IUserService service) => Login(user, service))
//    .Accepts<UserLogin>("application/json")
//    .Produces<string>();

app.MapGet("/desks/{startDate}/{endDate}", async (IDeskService deskService, DateTime startDate, DateTime endDate) => await deskService.GetAvailableDesks(startDate, endDate));


app.MapGet("/desks/", async (IDeskService deskService) => await deskService.GetAllDesks());

app.MapGet("/desk/{locationId}", async (IDeskService deskService, string locationId) => await deskService.GetDeskByOfficeId(locationId));


app.MapGet("/bookings", async (IBookingService bookingService) =>  await bookingService.GetAllBookings());

app.MapGet("/locations", async (ILocationProvider locationService) => await locationService.GetAllLocations());

app.MapPost("/booking/", async (IBookingService bookingService, Booking booking) => await bookingService.AddBooking(booking));

app.MapPost("/location", async (ILocationProvider locationService, Location location) => await locationService.AddLocation(location));

app.MapPut("/booking/{deskId}", async (IBookingService bookingService, Booking booking, string deskId) => await bookingService.UpdateBooking(booking, deskId));

app.MapDelete("/location/{locationId}", async (ILocationProvider locationService, string locationId) => await locationService.DeleteLocationById(locationId));

app.MapPost("/desk/", async (IDeskService deskService, Desk desk) => await deskService.AddDesk(desk));

app.MapDelete("/desk/deskId", async (IDeskService deskService, string deskId) => await deskService.DeleteDeskById(deskId));


app.Run();




