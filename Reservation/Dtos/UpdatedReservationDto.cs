namespace Reservation.Dtos;

public record class UpdatedReservationDto(
    int PatientId,
    int DoctorId,
    DateTime ReservationDate);
