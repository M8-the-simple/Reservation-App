using Reservation.Entities;

namespace Reservation.Endpoints;

public static class DoctorEndpoints
{
    const string GetPatientEndpointName = "DoctorGetGame";
    public static void MapDoctorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("doctor");
        
        group.MapGet("/", (ReservationAppContext dbContext) => dbContext.Doctors);

        // app.MapGet("/{id}", (int id) =>
        // {
        //     PatientDto? patient = patientDtos.Find(p => p.Id == id);

        //     return patient is null ?
        //     Results.NotFound() : Results.Ok($"Patient Id: {patient.Id}");

        // }).WithName(GetPatientEndpointName);

        // app.MapPut("/{id}", (PatientDto patient, int id) =>
        // {
        //     if (patientDtos.Find(p => p.Id == id) is null)
        //     {
        //         return Results.NotFound();
        //     }
            
        //     patientDtos[id - 1] = patient;
        //     return Results.NoContent();
        // });
    }

}
