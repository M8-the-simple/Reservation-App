using System;

namespace Reservation.Entities;

public class Patient
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
