# Backend Documentation

This project contains the RentiFul backend, a modular .NET 10 solution centered on a single API host that composes several domain modules for the rental platform.

## Architecture Overview

The backend uses an aggregator-host architecture:

- `applications/aggregator/API`: the executable ASP.NET Core host
- `applications/aggregator/SharedKernel`: shared backend infrastructure and cross-cutting helpers
- `applications/aggregator/SharedContracts`: shared contracts used across modules
- `modules/*`: domain modules that encapsulate specific business capabilities

At startup, the API host registers each module, configures shared middleware and infrastructure, maps module endpoints, and applies database migrations.

## Domain Modules

### Tenant Module

Handles tenant profile creation and updates, favorite properties, and the tenant's current residence relationships.

### Manager Module

Handles manager profile creation, retrieval, and updates.

### Property Module

Handles property creation, listing queries, manager property views, property details, and location-related property data.

### Lease Module

Handles lease creation, lease lookup for tenants and properties, and lease-related synchronization with other modules.

### Payment Module

Handles payment records tied to leases and exposes monthly payment status queries.

### Application Module

Handles rental application creation, tenant and manager application listings, status updates, lease linking, and saga-driven workflows for state transitions.

## Internal Module Structure

Most modules follow the same internal layout:

- `Domain`: entities and core business models
- `Data`: Entity Framework Core DbContexts, repository implementations, entity configurations, and migrations
- `Features`: use-case level handlers, endpoints, commands, queries, and module-facing services
- `Contracts`: DTOs shared with other modules or the API surface

This keeps business logic grouped by domain while preserving clear technical boundaries inside each module.

## API Host Responsibilities

The API project in `applications/aggregator/API` is responsible for:

- configuring Kestrel runtime ports and protocols
- enabling CORS for the configured frontend origin
- wiring JWT bearer authentication and role mapping
- enabling authorization
- applying request rate limiting
- adding Serilog-based request and application logging
- registering FastEndpoints and Swagger
- wiring module registrations and endpoint mapping
- configuring MassTransit with RabbitMQ
- applying database migrations for all registered modules on startup

## Communication Patterns

The backend combines several communication styles:

- HTTP endpoints for client-facing operations
- gRPC services for module-to-module communication inside the backend
- RabbitMQ events and MassTransit consumers for asynchronous workflows
- saga orchestration in the application module for multi-step application status changes

This mix allows the platform to keep synchronous user flows responsive while still supporting decoupled background coordination between modules.

## Runtime Dependencies

The backend expects these main external dependencies:

- PostgreSQL for persistence
- RabbitMQ for asynchronous messaging
- JWT metadata and issuer configuration for authentication
- Optional S3 configuration for property-related media storage

Key runtime configuration is defined through environment variables or `appsettings` values, including:

- `ConnectionStrings__*` for module databases and RabbitMQ
- `RabbitMq__Username` and `RabbitMq__Password`
- `AllowedOrigin`
- `Authentication__MetadataAddress`
- `Authentication__ValidIssuer`
- `Authentication__Audience`
- `S3Settings__Region`
- `S3Settings__BucketName`
- `S3Settings__AccessKey`
- `S3Settings__SecretAccessKey`

## Build, Run, and Test

### Build the solution

```bash
dotnet build server.sln
```

### Run the API host

```bash
dotnet run --project applications/aggregator/API/API.csproj
```

### Run tests

```bash
dotnet test server.sln
```

## Migrations

The existing modules use EF Core migrations scoped to each module project.

### Managers Module

```powershell
cd .\modules\manager\Managers\
dotnet ef migrations add AddedManagerSchema -c ManagerDbContext -p ..\Managers\Managers.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
dotnet ef database update -c ManagerDbContext -p ..\Managers\Managers.csproj -s ..\..\..\applications\aggregator\API\API.csproj
```

### Properties Module

```powershell
cd .\modules\property\Properties\
dotnet ef migrations add AddedPropertySchema -c PropertyDbContext -p ..\Properties\Properties.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
dotnet ef database update -c PropertyDbContext -p ..\Properties\Properties.csproj -s ..\..\..\applications\aggregator\API\API.csproj
```

### Leases Module

```powershell
cd .\modules\lease\Leases\
dotnet ef migrations add AddedLeaseSchema -c LeaseDbContext -p ..\Leases\Leases.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
dotnet ef database update -c LeaseDbContext -p ..\Leases\Leases.csproj -s ..\..\..\applications\aggregator\API\API.csproj
```

### Payments Module

```powershell
cd .\modules\payment\Payments\
dotnet ef migrations add AddedPaymentSchema -c PaymentDbContext -p ..\Payments\Payments.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
dotnet ef database update -c PaymentDbContext -p ..\Payments\Payments.csproj -s ..\..\..\applications\aggregator\API\API.csproj
```

### Applications Module

```powershell
cd .\modules\application\Applications\
dotnet ef migrations add AddedApplicationSchema -c ApplicationDbContext -p ..\Applications\Applications.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
dotnet ef database update -c ApplicationDbContext -p ..\Applications\Applications.csproj -s ..\..\..\applications\aggregator\API\API.csproj
```

## Local Development with Docker Compose

When running from the repository root with Docker Compose, the backend container:

- listens on `http://localhost:5168`
- connects to the shared PostgreSQL container
- connects to the RabbitMQ container
- runs startup migrations before serving requests

That makes the backend the integration point between the frontend, the database, the broker, and external services such as authentication providers or S3.
