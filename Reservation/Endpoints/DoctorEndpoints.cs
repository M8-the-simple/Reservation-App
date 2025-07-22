using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Reservation.Dtos;
using Reservation.Entities;
using Reservation.Mapping;
using Reservation.Utilities;

namespace Reservation.Endpoints;

public static class DoctorEndpoints
{
    const string GetPatientEndpointName = "DoctorGetReservation";
    public static void MapDoctorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("doctor").RequireAuthorization(policy =>
        {
            policy.RequireRole("doctor", "admin");
        });

        group.MapGet("/", (ReservationAppContext dbContext, ClaimsPrincipal user) =>
        {
            if(user.IsInRole("admin")) return Results.Ok(dbContext.Reservations.Select(r => r.ToReservationDetailsDto()));
            var hasClaim = user.HasClaim(claim => claim.Type == "id");
            if (hasClaim)
            {
                int id = Int32.Parse(user.FindFirstValue("id")!);
                var doctor = dbContext.Doctors.Find(id);
                if (doctor is null) return Results.NotFound("Doctor doesn't exist");
                return Results.Ok(dbContext.Reservations
                     .Where(r => r.DoctorId == id)
                     .Include(r => r.Patient)
                     .Include(r => r.Doctor)
                     .Select(r => r.ToReservationSummaryDto())
                     .ToList());    
            }
            return Results.NotFound("User has no claim 'id'");
            
        });
        group.MapPut("/reservation/{resId}", (int resId, ReservationAppContext dbContext, DoctorUpdateReservation update, ClaimsPrincipal user) =>
        {
            var hasClaim = user.HasClaim(claim => claim.Type == "id");
            if (hasClaim)
            {
                var patient = dbContext.Patients.Find(update.PatientId);
                if (patient is null) return Results.NotFound("Patient doesn't exist");
                var reservation = dbContext.Reservations.Find(resId);
                if (reservation is null) return Results.NotFound("Reservation not found");

                int docId = Int32.Parse(user.FindFirstValue("id")!);

                if (reservation.DoctorId != docId) return Results.NotFound($"Doctor with id: {docId} doesn't have reservation with id: {resId}"); 
                dbContext.Entry(reservation)
                         .CurrentValues
                         .SetValues(update.ToEntity(resId, reservation.DoctorId));
                dbContext.SaveChanges();

                return Results.NoContent();    
            }
            return Results.NotFound("No claim 'id' found");
        });
        group.MapDelete("/{resId}", (int resId, ReservationAppContext dbContext, ClaimsPrincipal user) =>
        {
            var hasClaim = user.HasClaim(claim => claim.Type == "id");
            if (hasClaim)
            {
                if (dbContext.DeleteReservation(user, resId)) return Results.NoContent();
                return Results.NotFound($"Doctor doesn't have reservation with id: {resId}");
            }
            return Results.NotFound("No claim 'id' found");       

        });
    }
    

}
