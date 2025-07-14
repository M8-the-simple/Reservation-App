using System;
using Microsoft.EntityFrameworkCore;
using Reservation.Entities;

namespace Reservation.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ReservationAppContext>();
            await dbContext.Database.MigrateAsync();
        }
}
