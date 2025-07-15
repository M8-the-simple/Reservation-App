namespace Reservation.Dtos;

public record class DoctorUpdateReservation(
    int PatientId,
    DateTime NewDate
);
