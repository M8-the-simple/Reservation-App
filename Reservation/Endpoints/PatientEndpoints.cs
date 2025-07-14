using System;
using Microsoft.EntityFrameworkCore;
using Reservation.Dtos;
using Reservation.Entities;
using Reservation.Mapping;
using Reservation.Utilities;

namespace Reservation.Endpoints;

public static class PatientEndpoints
{
    public static void MapPatientEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("patient");

        group.MapPost("/", (ReservationDetailsDto reservation, ReservationAppContext dbContext) =>
        {
            var patient = dbContext.Patients.Find(reservation.PatientId);
            var doctor = dbContext.Doctors.Find(reservation.DoctorId);

            if (patient is null || doctor is null) return Results.NotFound();

            ReservationEntity reservationEntity = reservation.ToEntity();

            dbContext.Reservations.Add(reservationEntity);

            dbContext.SaveChanges();

            return Results.Ok(reservationEntity);
        });

        group.MapGet("/{id}", (ReservationAppContext dbContext, int id) =>
        dbContext.Reservations
                 .Where(r => r.PatientId == id)
                 .Include(r => r.Patient)
                 .Include(r => r.Doctor)
                 .Select(r => r.ToReservationSummaryDto()));

        group.MapGet("/reservation/{Id}", (ReservationAppContext dbContext, int id) =>
        {
            var reservation = dbContext.Reservations.FirstOrDefault(r => r.Id == id);
            return reservation is null ? Results.NotFound() : Results.Ok(reservation.ToReservationDetailsDto());   
        });

        group.MapPut("/reservation/{resid}", (int resid, ReservationAppContext dbContext, UpdatedReservationDto update) =>
        {
            var patient = dbContext.Patients.Find(update.PatientId);
            var doctor = dbContext.Doctors.Find(update.DoctorId);

            if (patient is null || doctor is null) return Results.NotFound("Patient or doctor");

            var existingReservation = dbContext.Reservations
                                               .Where(r => r.PatientId == update.PatientId)
                                               .FirstOrDefault(r => r.Id == resid);

            if (existingReservation is null) return Results.NotFound("Reservation");

            dbContext.Entry(existingReservation)
                     .CurrentValues.SetValues(update.ToEntity(resid));

            dbContext.SaveChanges();

            return Results.NoContent();
        });
        group.MapDelete("/{id}/{resid}", (int id, int resid, ReservationAppContext dbContext) =>
        {
            var patient = dbContext.Patients.Find(id);
            if (patient is null) return Results.NotFound("Patient");

            var reservation = dbContext.Reservations.Where(r => r.PatientId == id).FirstOrDefault(r => r.Id == resid);
            if (reservation is null) return Results.NotFound("Reservation");

            dbContext.Reservations.Where(r => r.Id == resid).ExecuteDelete();

            return Results.NoContent();
        });
    }
}
