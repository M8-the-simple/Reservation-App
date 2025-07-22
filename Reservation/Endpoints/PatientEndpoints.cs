using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        string GetReservationEndpointName = "GetReservation";

        var group = app.MapGroup("patient").RequireAuthorization(policy =>
        {
            policy.RequireRole("patient", "admin");
        });

        group.MapPost("/", (ReservationDetailsDto reservation, ReservationAppContext dbContext, ClaimsPrincipal user) =>
        {
            var hasClaim = user.HasClaim(claim => claim.Type == "id");
            if (!hasClaim) return Results.NotFound("User has no claim 'id'");

            int id = Int32.Parse(user.FindFirstValue("id")!);

            var patient = dbContext.Patients.Find(id);
            var doctor = dbContext.Doctors.Find(reservation.DoctorId);

            if (patient is null || doctor is null) return Results.NotFound();

            ReservationEntity reservationEntity = reservation.ToEntity(id);

            dbContext.Reservations.Add(reservationEntity);

            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetReservationEndpointName, new { Id = reservationEntity.Id}, reservationEntity.ToReservationDetailsDto());
        });

        group.MapGet("/", (ReservationAppContext dbContext, ClaimsPrincipal user) =>
        {
            var hasClaim = user.HasClaim(claim => claim.Type == "id");
            if (hasClaim)
            {
                int id = Int32.Parse(user.FindFirstValue("id")!);
                
                return Results.Ok(dbContext.Reservations
                     .Where(r => r.PatientId == id)
                     .Include(r => r.Patient)
                     .Include(r => r.Doctor)
                     .Select(r => r.ToReservationSummaryDto()));
                
            }
            return Results.NotFound();
        });
        

        group.MapGet("/reservation/{Id}", [Authorize(Roles = "admin")] (ReservationAppContext dbContext, int id) =>
        {
            var reservation = dbContext.Reservations.FirstOrDefault(r => r.Id == id);
            return reservation is null ? Results.NotFound() : Results.Ok(reservation.ToReservationDetailsDto());
        }).WithName(GetReservationEndpointName);/*.RequireAuthorization(policy => policy.RequireRole("admin"));*/

        group.MapPut("/reservation/{resId}", (int resId, ClaimsPrincipal user, ReservationAppContext dbContext, UpdatedReservationDto update) =>
        {
            var existingReservation = dbContext.Reservations.Find(resId);
            var doctor = dbContext.Doctors.Find(update.DoctorId);

            if (existingReservation is null) return Results.NotFound("Reservation doesn't exist");
            if (doctor is null) return Results.NotFound("Doctor doesn't exist");


            if (user.FindFirstValue("role") != "admin")
            {
                var hasClaim = user.HasClaim(claim => claim.Type == "id");
                if (hasClaim)
                {
                    int id = Int32.Parse(user.FindFirstValue("id")!);
                    if (existingReservation.PatientId != id) return Results.NotFound("Unable to find your reservation");
                }
            }
            dbContext.Entry(existingReservation)
                     .CurrentValues.SetValues(update.ToEntity(resId, existingReservation.PatientId));

            dbContext.SaveChanges();

            return Results.NoContent();
        });
        group.MapDelete("/reservation/{resid}", (int resid, ReservationAppContext dbContext, ClaimsPrincipal user) =>
        {
            var hasClaim = user.HasClaim(claim => claim.Type == "id");
            if (hasClaim)
            {
                if(dbContext.DeleteReservation(user, resid)) return Results.NoContent();

                return Results.NotFound($"You don't have the reservation with id: {resid}");                
            }
            return Results.NotFound("No claim: 'id'");
        });
    }
}
