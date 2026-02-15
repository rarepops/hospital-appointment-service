# Hospital Appointment Service

A .NET 8 Web API for scheduling hospital appointments, built with **Clean Architecture** and the **Strategy Pattern** for extensible department-specific validation.

## Architecture

## Supported Departments

| Department | Validation Rules |
|---|---|
| General Practice | Patient must be assigned to the GP |
| Physiotherapy | Requires doctor's referral + insurance approval |
| Radiology | Requires doctor's referral + financial approval |
| Surgery | Requires specialist referral |

Adding a new department requires only:

1. A new `IDepartmentValidator` implementation in `Hospital.Infrastructure/Validators/`
2. Registering it in `Hospital.Infrastructure/DependencyInjection.cs`

No changes to core logic needed.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Getting Started

```bash
# Clone the repository
git clone <repository-url>
cd "Hospital Appointment Service"

# Restore dependencies
dotnet restore

# Trust the HTTPS development certificate (required once)
dotnet dev-certs https --trust

# Run the application
dotnet run --project Hospital.WebApi
```
