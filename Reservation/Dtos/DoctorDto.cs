namespace Reservation.Dtos;

public record class DoctorDto(
    int Id,
    string FirstName,
    string LastName,
    string Patient,
    DateTime ReservationDate
);
