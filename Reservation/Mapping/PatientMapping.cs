using System;
using Reservation.Dtos;
using Reservation.Entities;

namespace Reservation.Mapping;

public static class PatientMapping
{
    public static Patient ToEntity(this PatientDto patient)
    {
        return new Patient() { FirstName = patient.FirstName, LastName = patient.LastName };
    } 
}
