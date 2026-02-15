using Hospital.Application.Commands;
using Hospital.Domain.Interfaces;

namespace Hospital.WebApi.Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/appointments");

        group.MapGet(
            "/",
            async (IAppointmentRepository repository) =>
            {
                var appointments = await repository.GetAllAsync();
                return Results.Ok(appointments);
            }
        );

        group.MapGet(
            "/{id:int}",
            async (int id, IAppointmentRepository repository) =>
            {
                var appointment = await repository.GetByIdAsync(id);
                return appointment is not null ? Results.Ok(appointment) : Results.NotFound();
            }
        );

        group.MapPost(
            "/",
            async (AppointmentRequest request, ScheduleAppointmentHandler handler) =>
            {
                var command = new ScheduleAppointmentCommand(
                    request.Cpr,
                    request.PatientName,
                    request.AppointmentDate,
                    request.Department,
                    request.DoctorName
                );

                var result = await handler.HandleAsync(command);

                return result.IsSuccess
                    ? Results.Ok("Appointment scheduled successfully.")
                    : Results.BadRequest(result.ErrorMessage);
            }
        );

        group.MapDelete(
            "/{id:int}",
            async (int id, IAppointmentRepository repository) =>
            {
                await repository.DeleteAsync(id);
                return Results.NoContent();
            }
        );
    }
}
