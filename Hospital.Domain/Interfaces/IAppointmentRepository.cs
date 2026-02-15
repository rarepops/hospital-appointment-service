using Hospital.Domain.Entities;
using NodaTime;

namespace Hospital.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id);
    Task<IReadOnlyList<Appointment>> GetAllAsync();
    Task<bool> ExistsAsync(string cpr, string department, Instant appointmentDate);
    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(int id);
}
