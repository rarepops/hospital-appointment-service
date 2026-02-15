using Hospital.Application;
using Hospital.Application.Commands;
using Hospital.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Register layers via Clean Architecture DI extensions
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost(
    "/appointments",
    async (AppointmentRequest request, ScheduleAppointmentHandler handler) =>
    {
        var command = new ScheduleAppointmentCommand(
            request.Cpr,
            request.PatientName,
            request.AppointmentDate,
            request.Department,
            request.DoctorName
        );

        var result = await handler.HandleAsync(command);

        return result.IsSuccess
            ? Results.Ok("Appointment scheduled successfully.")
            : Results.BadRequest(result.ErrorMessage);
    }
);

app.Run();

public record AppointmentRequest(
    string Cpr,
    string PatientName,
    DateTime AppointmentDate,
    string Department,
    string DoctorName
);
