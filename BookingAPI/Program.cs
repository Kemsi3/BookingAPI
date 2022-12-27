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

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorisation",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT authentication for MinimalAPI"
};

var securityRequirements = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }
};

var info = new OpenApiInfo()
{
    Version = "V1",
    Title = "BookingApi with JWT Authentication",
    Description = "BookingApi with JWT Authentication"
};

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", info);
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(securityRequirements);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    ValidateAudience = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true

});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();


app.UseCors(MyAllowSpecificOrigins);

app.MapPost("/login", [AllowAnonymous] (IUserService userService, UserLogin user) =>
{
   
    var loggedUser = userService.Get(user);

    if(loggedUser != null)
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var tokenDesciptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Name)
            }),

            Expires = DateTime.Now.AddMinutes(15),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = credentials
        };

        var token = jwtTokenHandler.CreateToken(tokenDesciptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return Results.Ok(jwtToken);
    }
    return Results.Unauthorized();
});


app.MapGet("/desks/{startDate}/{endDate}", [AllowAnonymous] async (IDeskService deskService, DateTime startDate, DateTime endDate) => await deskService.GetAvailableDesks(startDate, endDate));


app.MapGet("/desks/",  async (IDeskService deskService) => await deskService.GetAllDesks());

app.MapGet("/desk/{locationId}", async (IDeskService deskService, string locationId) => await deskService.GetDeskByOfficeId(locationId));


app.MapGet("/bookings", [Authorize] async (IBookingService bookingService) =>  await bookingService.GetAllBookings());

app.MapGet("/locations", async (ILocationProvider locationService) => await locationService.GetAllLocations());

app.MapPost("/booking/", async (IBookingService bookingService, Booking booking) => await bookingService.AddBooking(booking));

app.MapPost("/location", [Authorize] async (ILocationProvider locationService, Location location) => await locationService.AddLocation(location));

app.MapPut("/booking/{deskId}", async (IBookingService bookingService, Booking booking, string deskId) => await bookingService.UpdateBooking(booking, deskId));

app.MapDelete("/location/{locationId}", [Authorize] async (ILocationProvider locationService, string locationId) => await locationService.DeleteLocationById(locationId));

app.MapPost("/desk/", [Authorize] async (IDeskService deskService, Desk desk) => await deskService.AddDesk(desk));

app.MapDelete("/desk/deskId", [Authorize] async (IDeskService deskService, string deskId) => await deskService.DeleteDeskById(deskId));

app.UseSwagger();
app.UseSwaggerUI();

app.Run();




