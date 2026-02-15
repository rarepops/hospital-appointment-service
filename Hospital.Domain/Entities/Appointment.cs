using NodaTime;

namespace Hospital.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public string Cpr { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public Instant AppointmentDate { get; set; }
    public string Department { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
}
