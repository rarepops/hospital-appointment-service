namespace Hospital.Application;

public class AppointmentRepository(AppointmentDbContext dbContext)
{
    public async Task AddAsync(Appointment appointment)
    {
        dbContext.Appointments.Add(appointment);
        await dbContext.SaveChangesAsync();
    }
}