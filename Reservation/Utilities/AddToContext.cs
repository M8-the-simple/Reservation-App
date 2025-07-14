using System;
using Reservation.Dtos;
using Reservation.Entities;

namespace Reservation.Utilities;

public static class AddToContext
{
    public static void AddAllToContext(ReservationEntity reservation, ReservationAppContext dbContext)
     {
         dbContext.Reservations.Add(reservation);
    //     dbContext!.Patients.First(p => p.Id == reservation.PatientId)!.Reservations!.Add(reservation);
    //     dbContext!.Doctors.Find(reservation.PatientId)!.Reservations!.Add(reservation);
    //     dbContext!.SaveChanges();
     }
    // public static void UpdateAll(ReservationEntity reservation, ReservationAppContext dbContext)
    // {
    //     dbContext.Reservations.Add(reservation);
    //     dbContext!.Patients.First(p => p.Id == reservation.PatientId)!.Reservations!.Add(reservation);
    //     dbContext!.Doctors.Find(reservation.PatientId)!.Reservations!.Add(reservation);
    //     dbContext!.SaveChanges();
    // }
    
}
