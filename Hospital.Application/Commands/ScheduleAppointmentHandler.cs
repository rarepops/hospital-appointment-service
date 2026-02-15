using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Domain.Results;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace Hospital.Application.Commands;

/// <summary>
/// Handles scheduling an appointment by orchestrating validation and persistence.
/// Department-specific rules are delegated to <see cref="IDepartmentValidator"/> implementations,
/// so adding a new department never requires modifying this class.
/// </summary>
public class ScheduleAppointmentHandler(
    IAppointmentRepository appointmentRepository,
    INationalRegistryService nationalRegistryService,
    IEnumerable<IDepartmentValidator> departmentValidators,
    IClock clock,
    ILogger<ScheduleAppointmentHandler> logger
)
{
    private readonly Dictionary<string, IDepartmentValidator> _validators = departmentValidators.ToDictionary(
        v => v.Department,
        StringComparer.OrdinalIgnoreCase
    );

    public async Task<Result> HandleAsync(ScheduleAppointmentCommand command)
    {
        if (
            string.IsNullOrWhiteSpace(command.Cpr)
            || string.IsNullOrWhiteSpace(command.Department)
            || string.IsNullOrWhiteSpace(command.DoctorName)
            || command.AppointmentDate <= clock.GetCurrentInstant()
        )
        {
            logger.LogWarning("Invalid appointment request received.");
            return Result.Failure(
                "Invalid appointment request. Ensure all fields are provided and the date is in the future."
            );
        }

        if (!await nationalRegistryService.ValidateCpr(command.Cpr))
        {
            logger.LogWarning("CPR validation failed for {Cpr}.", command.Cpr);
            return Result.Failure("Invalid CPR number. Cannot schedule appointment.");
        }

        if (!_validators.TryGetValue(command.Department, out var validator))
        {
            logger.LogWarning("No validator registered for department {Department}.", command.Department);
            return Result.Failure($"Unsupported department: {command.Department}.");
        }

        var departmentResult = await validator.ValidateAsync(command.Cpr, command.DoctorName);
        if (!departmentResult.IsValid)
        {
            logger.LogWarning(
                "Department validation failed for {Department}: {Error}",
                command.Department,
                departmentResult.ErrorMessage
            );
            return Result.Failure(departmentResult.ErrorMessage!);
        }

        // Check for duplicate appointment
        if (await appointmentRepository.ExistsAsync(command.Cpr, command.Department, command.AppointmentDate))
        {
            logger.LogWarning(
                "Duplicate appointment for {Cpr} in {Department} on {Date}.",
                command.Cpr,
                command.Department,
                command.AppointmentDate
            );
            return Result.Failure(
                "An appointment already exists for this patient in the same department at the specified time."
            );
        }

        var appointment = new Appointment
        {
            Cpr = command.Cpr,
            PatientName = command.PatientName,
            AppointmentDate = command.AppointmentDate,
            Department = command.Department,
            DoctorName = command.DoctorName,
        };

        await appointmentRepository.AddAsync(appointment);

        logger.LogInformation(
            "Appointment scheduled for {PatientName} (CPR: {Cpr}) in {Department} with {DoctorName} on {Date}.",
            command.PatientName,
            command.Cpr,
            command.Department,
            command.DoctorName,
            command.AppointmentDate
        );

        return Result.Success();
    }
}
