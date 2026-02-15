using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Persistence;
using Hospital.Infrastructure.Services;
using Hospital.Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Infrastructure layer services: persistence, external services, and department validators.
    /// To add a new department, simply register an additional <see cref="IDepartmentValidator"/> implementation here.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        services.AddDbContext<AppointmentDbContext>(options => options.UseInMemoryDatabase("HospitalDb"));
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        // External services
        services.Configure<NationalRegistryOptions>(configuration.GetSection(NationalRegistryOptions.SectionName));
        services.AddHttpClient("NationalRegistry");
        services.AddScoped<INationalRegistryService, NationalRegistryService>();

        // Department validators
        services.AddScoped<IDepartmentValidator, GeneralPracticeValidator>();
        services.AddScoped<IDepartmentValidator, PhysiotherapyValidator>();
        services.AddScoped<IDepartmentValidator, SurgeryValidator>();
        services.AddScoped<IDepartmentValidator, RadiologyValidator>();

        return services;
    }
}
