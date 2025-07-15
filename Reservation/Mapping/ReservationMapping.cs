using System;
using Reservation.Dtos;
using Reservation.Entities;

namespace Reservation.Mapping;

public static class ReservationMapping
{
    public static ReservationEntity ToEntity(this ReservationDetailsDto reservation)
    {
        return new ReservationEntity()
        {
            PatientId = reservation.PatientId,
            DoctorId = reservation.DoctorId,
            Date = reservation.ReservationDate
        };
    }
    public static ReservationEntity ToEntity(this UpdatedReservationDto reservation, int id, int patientId)
    {
        return new ReservationEntity()
        {
            Id = id,
            PatientId = patientId,
            DoctorId = reservation.DoctorId,
            Date = reservation.ReservationDate
        };
    }
    public static ReservationEntity ToEntity(this DoctorUpdateReservation reservation, int id, int doctorId)
    {
        return new ReservationEntity()
        {
            Id = id,
            PatientId = reservation.PatientId,
            DoctorId = doctorId,
            Date = reservation.NewDate
        };
    }
    public static ReservationDetailsDto ToReservationDetailsDto(this ReservationEntity entity)
    {
        return new ReservationDetailsDto(
            entity.Id,
            entity.PatientId,
            entity.DoctorId,
            entity.Date
        );
    }
    public static ReservationSummaryDto ToReservationSummaryDto(this ReservationEntity entity)
    {
        return new ReservationSummaryDto(
            entity.Id,
            entity.Patient!.FirstName,
            entity.Doctor!.FirstName,
            entity.Date
        );
    }
}
