namespace Reservation.Dtos;

public record class ReservationSummaryDto(
    int Id,
    string PatientName,
    string DoctorName,
    DateTime ReservationDate
);
