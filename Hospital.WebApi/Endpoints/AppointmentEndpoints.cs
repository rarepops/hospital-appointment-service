using Hospital.Application.Commands;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.WebApi.Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/appointments").WithTags("Appointments");

        group
            .MapGet(
                "/",
                async (IAppointmentRepository repository, CancellationToken cancellationToken) =>
                {
                    var appointments = await repository.GetAllAsync(cancellationToken);
                    return Results.Ok(appointments);
                }
            )
            .Produces<IReadOnlyList<Appointment>>();

        group
            .MapGet(
                "/{id:int}",
                async (int id, IAppointmentRepository repository, CancellationToken cancellationToken) =>
                {
                    var appointment = await repository.GetByIdAsync(id, cancellationToken);
                    return appointment is not null ? Results.Ok(appointment) : Results.NotFound();
                }
            )
            .Produces<Appointment>()
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapPost(
                "/",
                async (
                    AppointmentRequest request,
                    ScheduleAppointmentHandler handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new ScheduleAppointmentCommand(
                        request.Cpr,
                        request.PatientName,
                        request.AppointmentDate,
                        request.Department,
                        request.DoctorName
                    );

                    var result = await handler.HandleAsync(command, cancellationToken);

                    return result.IsSuccess
                        ? Results.Created($"/appointments/{result.Data!.Id}", result.Data)
                        : Results.BadRequest(result.ErrorMessage);
                }
            )
            .Produces<Appointment>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group
            .MapPut(
                "/{id:int}",
                async (
                    int id,
                    AppointmentRequest request,
                    IAppointmentRepository repository,
                    INationalRegistryService registryService,
                    CancellationToken cancellationToken
                ) =>
                {
                    var existing = await repository.GetByIdAsync(id, cancellationToken);
                    if (existing is null)
                        return Results.NotFound();

                    if (
                        string.IsNullOrWhiteSpace(request.Cpr)
                        || string.IsNullOrWhiteSpace(request.Department)
                        || string.IsNullOrWhiteSpace(request.DoctorName)
                    )
                        return Results.BadRequest("All fields are required.");

                    if (!await registryService.ValidateCpr(request.Cpr))
                        return Results.BadRequest("Invalid CPR number.");

                    existing.Cpr = request.Cpr;
                    existing.PatientName = request.PatientName;
                    existing.AppointmentDate = request.AppointmentDate;
                    existing.Department = request.Department;
                    existing.DoctorName = request.DoctorName;

                    await repository.UpdateAsync(existing, cancellationToken);
                    return Results.Ok(existing);
                }
            )
            .Produces<Appointment>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/{id:int}",
                async (int id, IAppointmentRepository repository, CancellationToken cancellationToken) =>
                {
                    var deleted = await repository.DeleteAsync(id, cancellationToken);
                    return deleted ? Results.NoContent() : Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
