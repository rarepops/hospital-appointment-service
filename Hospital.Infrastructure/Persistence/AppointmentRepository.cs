using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace Hospital.Infrastructure.Persistence;

public class AppointmentRepository(AppointmentDbContext dbContext, IFusionCache cache) : IAppointmentRepository
{
    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await cache.GetOrSetAsync(
            $"appointment:{id}",
            async _ => await dbContext.Appointments.FindAsync(id),
            TimeSpan.FromMinutes(5)
        );
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync()
    {
        return await cache.GetOrSetAsync(
            "appointments:all",
            async _ => (IReadOnlyList<Appointment>)await dbContext.Appointments.ToListAsync(),
            TimeSpan.FromMinutes(2)
        );
    }

    public async Task AddAsync(Appointment appointment)
    {
        dbContext.Appointments.Add(appointment);
        await dbContext.SaveChangesAsync();
        await cache.RemoveAsync("appointments:all");
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        dbContext.Appointments.Update(appointment);
        await dbContext.SaveChangesAsync();
        await cache.RemoveAsync($"appointment:{appointment.Id}");
        await cache.RemoveAsync("appointments:all");
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await dbContext.Appointments.FindAsync(id);
        if (appointment is not null)
        {
            dbContext.Appointments.Remove(appointment);
            await dbContext.SaveChangesAsync();
            await cache.RemoveAsync($"appointment:{id}");
            await cache.RemoveAsync("appointments:all");
        }
    }
}
