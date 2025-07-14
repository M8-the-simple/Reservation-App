using System;
using Microsoft.EntityFrameworkCore;

namespace Reservation.Entities;

public class ReservationAppContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(
            new { Id = 1, FirstName = "Mike", LastName = "Meyers" },
            new { Id = 2, FirstName = "Charles", LastName = "Manson" },
            new { Id = 3, FirstName = "Maxim", LastName = "House" });

        modelBuilder.Entity<Patient>().HasData(
            new { Id = 1, FirstName = "Julia", LastName = "Jules" },
            new { Id = 2, FirstName = "Jack", LastName = "Jones" },
            new { Id = 3, FirstName = "Miles", LastName = "Morales" }
        );
    }
}
