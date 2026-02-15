using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Persistence;

public class AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : DbContext(options)
{
    public DbSet<Appointment> Appointments => Set<Appointment>();
}
