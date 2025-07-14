using System;

namespace Reservation.Dtos;

public record class ReservationDetailsDto(
    int Id,
    int PatientId,
    int DoctorId,
    DateTime ReservationDate);
