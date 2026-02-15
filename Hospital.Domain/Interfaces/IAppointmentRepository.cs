using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id);
    Task<IReadOnlyList<Appointment>> GetAllAsync();
    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(int id);
}
