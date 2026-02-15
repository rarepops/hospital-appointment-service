using NodaTime;

namespace Hospital.WebApi.Endpoints;

public record AppointmentRequest(
    string Cpr,
    string PatientName,
    Instant AppointmentDate,
    string Department,
    string DoctorName
);
