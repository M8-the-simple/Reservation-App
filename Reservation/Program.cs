using Microsoft.AspNetCore.Authentication.JwtBearer;
using Reservation.Data;
using Reservation.Endpoints;
using Reservation.Entities;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DatabaseString");
builder.Services.AddSqlite<ReservationAppContext>(connString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDoctorEndpoints();
app.MapPatientEndpoints();

await app.MigrateDbAsync();

app.Run();
