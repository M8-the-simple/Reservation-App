using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Reservation.Dtos;
using Reservation.Entities;
using Reservation.Mapping;

namespace Reservation.Endpoints;

public static class DoctorEndpoints
{
    const string GetPatientEndpointName = "DoctorGetReservation";
    public static void MapDoctorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("doctor");

        group.MapGet("/{id}", (int id, ReservationAppContext dbContext) =>
        {
            var doctor = dbContext.Doctors.Find(id);
            if (doctor is null) return Results.NotFound();
            return Results.Ok(dbContext.Reservations
                     .Where(r => r.DoctorId == id)
                     .Include(r => r.Patient)
                     .Include(r => r.Doctor)
                     .Select(r => r.ToReservationSummaryDto())
                     .ToList());
        });
        group.MapPut("/reservation/{id}", (int id, ReservationAppContext dbContext, DoctorUpdateReservation update) =>
        {
            var reservation = dbContext.Reservations.Where(r => r.PatientId == update.PatientId).FirstOrDefault(d => d.Id == id);
            if (reservation is null) return Results.NotFound("Reservation or patient");

            dbContext.Entry(reservation)
                     .CurrentValues
                     .SetValues(update.ToEntity(id, reservation.DoctorId));
            dbContext.SaveChanges();

            return Results.NoContent();
        });
        group.MapDelete("/{docId}/{resId}", (int docId, int resId, ReservationAppContext dbContext) =>
        {
            var reservation = dbContext.Reservations.Where(r => r.DoctorId == docId).FirstOrDefault(r => r.Id == resId);
            if (reservation is null) return Results.NotFound();

            dbContext.Reservations.Remove(reservation!);
            dbContext.SaveChanges();
            return Results.NoContent();

        });
    }
    

}
