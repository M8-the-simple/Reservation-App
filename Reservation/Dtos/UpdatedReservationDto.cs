namespace Reservation.Dtos;

public record class UpdatedReservationDto(
    int DoctorId,
    DateTime ReservationDate);
