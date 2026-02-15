using Hospital.Application.Commands;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Hospital.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IClock>(SystemClock.Instance);
        services.AddScoped<ScheduleAppointmentHandler>();
        return services;
    }
}
