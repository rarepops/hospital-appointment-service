using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using ZiggyCreatures.Caching.Fusion;

namespace Hospital.Infrastructure.Persistence;

public class AppointmentRepository(AppointmentDbContext dbContext, IFusionCache cache) : IAppointmentRepository
{
    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await cache.GetOrSetAsync(
            $"appointment:{id}",
            async _ => await dbContext.Appointments.FindAsync([id], cancellationToken),
            TimeSpan.FromMinutes(5),
            token: cancellationToken
        );
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await cache.GetOrSetAsync(
            "appointments:all",
            async _ => (IReadOnlyList<Appointment>)await dbContext.Appointments.ToListAsync(cancellationToken),
            TimeSpan.FromMinutes(2),
            token: cancellationToken
        );
    }

    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        dbContext.Appointments.Add(appointment);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync("appointments:all", token: cancellationToken);
        return appointment;
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        dbContext.Appointments.Update(appointment);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync($"appointment:{appointment.Id}", token: cancellationToken);
        await cache.RemoveAsync("appointments:all", token: cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await dbContext.Appointments.FindAsync([id], cancellationToken);
        if (appointment is null)
            return false;

        dbContext.Appointments.Remove(appointment);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync($"appointment:{id}", token: cancellationToken);
        await cache.RemoveAsync("appointments:all", token: cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(
        string cpr,
        string department,
        Instant appointmentDate,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext.Appointments.AnyAsync(
            a => a.Cpr == cpr && a.Department == department && a.AppointmentDate == appointmentDate,
            cancellationToken
        );
    }
}
