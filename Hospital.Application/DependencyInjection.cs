using Hospital.Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Application layer services.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ScheduleAppointmentHandler>();
        return services;
    }
}
