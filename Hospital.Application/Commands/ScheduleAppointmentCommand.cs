namespace Hospital.Application.Commands;

/// <summary>
/// Represents a request to schedule an appointment.
/// </summary>
public record ScheduleAppointmentCommand(
    string Cpr,
    string PatientName,
    DateTime AppointmentDate,
    string Department,
    string DoctorName
);
