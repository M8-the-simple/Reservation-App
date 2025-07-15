using System;
using Microsoft.EntityFrameworkCore;
using Reservation.Dtos;
using Reservation.Entities;
using Reservation.Mapping;

namespace Reservation.Endpoints;

public static class PatientEndpoints
{
    
    public static void MapPatientEndpoints(this WebApplication app)
    {
        string GetReservationEndpointName = "GetReservation";
        var group = app.MapGroup("patient");

        group.MapPost("/", (ReservationDetailsDto reservation, ReservationAppContext dbContext) =>
        {
            var patient = dbContext.Patients.Find(reservation.PatientId);
            var doctor = dbContext.Doctors.Find(reservation.DoctorId);

            if (patient is null || doctor is null) return Results.NotFound();

            ReservationEntity reservationEntity = reservation.ToEntity();

            dbContext.Reservations.Add(reservationEntity);

            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetReservationEndpointName, new {Id = reservationEntity.Id}, reservationEntity.ToReservationDetailsDto());
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
        }).WithName(GetReservationEndpointName);

        group.MapPut("/reservation/{resId}", (int resId, ReservationAppContext dbContext, UpdatedReservationDto update) =>
        {
            var existingReservation = dbContext.Reservations.Where(r => r.Id == resId).FirstOrDefault(r => r.DoctorId == update.DoctorId);

            if (existingReservation is null) return Results.NotFound("Reservation or Doctor");

            dbContext.Entry(existingReservation)
                     .CurrentValues.SetValues(update.ToEntity(resId, existingReservation.PatientId));

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
