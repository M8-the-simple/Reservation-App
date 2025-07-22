using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Reservation.Entities;

namespace Reservation.Utilities;

public static class DeleteEntity
{
    public static bool DeleteReservation(this ReservationAppContext dbContext, ClaimsPrincipal user, int resid)
    {
        int id = Int32.Parse(user.FindFirstValue("id")!);

        var reservation = dbContext.Reservations.Where(r => r.PatientId == id).FirstOrDefault(r => r.Id == resid);
        if (reservation is null) return false;

        dbContext.Reservations.Where(r => r.Id == resid).ExecuteDelete();
        return true;
    }
}
