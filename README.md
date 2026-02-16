# Hospital Appointment Service

A .NET 8 Web API for scheduling hospital appointments, built with **Clean Architecture** and the **Strategy Pattern** for extensible department-specific validation.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (v8.0.418 or later, via `global.json`)

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

The API will be available at:

| Profile | URL |
|---|---|
| HTTP | `http://localhost:5082` |
| HTTPS | `https://localhost:7110` |

Swagger UI is available at the root URL in Development mode.

### Running with Aspire Dashboard

The solution includes [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/) for observability (traces, metrics, logs, health checks) via a local dashboard.

```bash
# Run via the AppHost
dotnet run --project Hospital.AppHost
```

The Aspire dashboard will be available at:

| Profile | Dashboard | OTLP Endpoint |
|---|---|---|
| HTTP | `http://localhost:15056` | `http://localhost:19159` |
| HTTPS | `https://localhost:17286` | `https://localhost:21227` |

## Architecture

The diagram below shows the **C4 Level 1 — System Context**, which provides a high-level overview of the system, its users, and external dependencies. See [c4model.com](https://c4model.com/) for more on the C4 model.

<img src="Docs/Hospital%20Appointment%20Service%20-%20C4%20Level%201.jpg" alt="C4 Level 1 — System Context" width="600">

The solution follows **Clean Architecture** with four layers:

| Project | Responsibility |
|---|---|
| `Hospital.Domain` | Entities, repository interfaces, domain models |
| `Hospital.Application` | Use cases (command handlers), business orchestration |
| `Hospital.Infrastructure` | Data access, external services, department validators |
| `Hospital.WebApi` | Minimal API endpoints, Swagger, HTTP pipeline |

Supporting projects:

- `Hospital.AppHost` — .NET Aspire orchestration for local observability
- `Hospital.ServiceDefaults` — Shared defaults for health checks, telemetry, and resilience

## API Endpoints

All endpoints are grouped under `/appointments`.

| Method | Route | Description | Success | Error |
|---|---|---|---|---|
| `GET` | `/appointments` | List all appointments | `200 OK` | — |
| `GET` | `/appointments/{id}` | Get appointment by ID | `200 OK` | `404 Not Found` |
| `POST` | `/appointments` | Schedule a new appointment | `201 Created` | `400 Bad Request` |
| `PUT` | `/appointments/{id}` | Update an existing appointment | `200 OK` | `400` / `404` |
| `DELETE` | `/appointments/{id}` | Delete an appointment | `204 No Content` | `404 Not Found` |

### Request Body (POST / PUT)

```json
{
  "Cpr": "123456-7890",
  "PatientName": "John Doe",
  "AppointmentDate": "2026-03-01T10:00:00Z",
  "Department": "Radiology",
  "DoctorName": "Dr. Smith"
}
```

> Dates use the [NodaTime](https://nodatime.org/) `Instant` format (ISO 8601 UTC).

A ready-to-use `.http` file with sample requests is available at `Hospital.WebApi/Hospital.WebApi.http`.

## Configuration

### National Registry Service

The API integrates with an external National Registry for CPR validation. Configure the connection in `appsettings.json` (or preferably via [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)):

```json
{
  "NationalRegistry": {
    "ApiKey": "<your-api-key>",
    "BaseUrl": "<registry-base-url>"
  }
}
```

### Database

The application uses an **EF Core in-memory database** (`HospitalDb`) by default — no external database setup is required.

### Caching

Response caching is provided by [FusionCache](https://github.com/ZiggyCreatures/FusionCache) with a 5-minute default TTL and 1-hour fail-safe window.

## Supported Departments

| Department | Validation Rules |
|---|---|
| General Practice | Patient must be assigned to the GP |
| Physiotherapy | Requires doctor's referral + insurance approval |
| Radiology | Requires doctor's referral + financial approval |
| Surgery | Requires specialist referral |

### Adding a New Department

1. Create a new `IDepartmentValidator` implementation in `Hospital.Infrastructure/Validators/`
2. Register it in `Hospital.Infrastructure/DependencyInjection.cs`

## Code Formatting

This project uses [CSharpier](https://csharpier.com/) for consistent code formatting.

```bash
# Install (first time only)
dotnet tool restore

# Format all files
dotnet csharpier format .

# Check formatting without modifying
dotnet csharpier check .
```

## Docker

```bash
# Build the image
docker build -t hospital-appointment-service .

# Run the container
docker run -p 8080:8080 hospital-appointment-service
```

## CI/CD

A GitHub Actions workflow runs on every push and pull request to `main`. It restores, builds the solution, and builds the Docker image. See [.github/workflows/build.yml](.github/workflows/build.yml).
