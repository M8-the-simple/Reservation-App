using Reservation.Data;
using Reservation.Endpoints;
using Reservation.Entities;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DatabaseString");
builder.Services.AddSqlite<ReservationAppContext>(connString);

var app = builder.Build();

app.MapDoctorEndpoints();
app.MapPatientEndpoints();

await app.MigrateDbAsync();

app.Run();
