using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Persistence;

public class AppointmentRepository(AppointmentDbContext dbContext) : IAppointmentRepository
{
    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await dbContext.Appointments.FindAsync(id);
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync()
    {
        return await dbContext.Appointments.ToListAsync();
    }

    public async Task AddAsync(Appointment appointment)
    {
        dbContext.Appointments.Add(appointment);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        dbContext.Appointments.Update(appointment);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await dbContext.Appointments.FindAsync(id);
        if (appointment is not null)
        {
            dbContext.Appointments.Remove(appointment);
            await dbContext.SaveChangesAsync();
        }
    }
}
