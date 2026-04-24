# RentiFul

RentiFul is a full-stack rental management platform for discovering properties, submitting rental applications, and managing the relationship between tenants and property managers. The repository contains a Next.js frontend, a modular .NET backend, and Docker Compose infrastructure for running the full stack locally.

## Technologies Used

### Frontend

- Next.js 15 with the App Router
- React 19
- TypeScript
- Tailwind CSS
- Redux Toolkit and RTK Query
- AWS Amplify UI for authentication flows
- Radix UI primitives
- Framer Motion
- Leaflet and MapTiler for map-based property discovery

### Backend

- .NET 10
- ASP.NET Core
- FastEndpoints
- Entity Framework Core
- Mediator
- MassTransit
- RabbitMQ
- PostgreSQL
- Serilog
- JWT Bearer authentication

### Infrastructure and Tooling

- Docker and Docker Compose
- ESLint
- xUnit
- Coverlet

## Features

### Public Discovery

- Landing page: introduces the product, key value proposition, and entry points for renters and managers.
- Property search: lets users browse listings with filters such as location, price range, bedrooms, bathrooms, square footage, property type, and amenities.
- Map view: pairs listing exploration with an interactive map for location-based browsing.
- Property detail pages: show images, highlights, property metadata, location information, and the application entry point.

### Tenant Experience

- Authentication-aware onboarding: signed-in users are resolved against the backend and created on demand if they do not exist yet.
- Favorites: tenants can save and remove favorite properties.
- Current residences: tenants can review the properties currently associated with their profile.
- Applications dashboard: tenants can track submitted applications and their current status.
- Settings management: tenants can update their account details from the dashboard.

### Manager Experience

- Manager profile provisioning: manager accounts are resolved from the identity provider and synchronized with backend records.
- Property management: managers can create new listings and review properties assigned to them.
- Applications dashboard: managers can review tenant applications and update their status.
- Settings management: managers can maintain their profile information.

### Operational Domain Features

- Lease management: the backend tracks leases linked to tenants and properties.
- Payment status tracking: payment records are associated with leases for rent follow-up workflows.
- Event-driven workflows: application, lease, property, tenant, and payment modules communicate through RabbitMQ and MassTransit.

## How The Project Can Be Improved

### User Experience

- Add stronger search features such as saved searches, sorting options, and richer geospatial filtering.
- Add real-time notifications for application updates, lease changes, and payment reminders.
- Improve the property detail experience with availability calendars, richer media galleries, and nearby location insights.
- Add dedicated manager workflows for editing listings, archiving listings, and viewing application pipelines by property.
- Add tenant-facing payment history, due-date reminders, and clearer lease timelines.

### Resilience and Development Lifecycle

- Add frontend tests for critical flows such as search, applications, and dashboard settings.
- Expand backend automated tests beyond the current integration coverage.
- Introduce CI pipelines for linting, building, testing, and container verification on every change.
- Add centralized observability with structured logs, metrics, health checks, and distributed tracing.
- Add secrets management and environment-specific deployment configuration for production readiness.
- Add caching, retry policies, and circuit-breaker patterns around external integrations.

## Run Locally With Docker Compose

### Prerequisites

1. Install Docker Desktop or Docker Engine with Docker Compose support.
2. Make sure ports `3000`, `5168`, `5432`, `5672`, and `15672` are available.
3. If you want authentication, map, or S3-backed media features to work end to end, gather the required AWS Cognito, MapTiler, and S3 values.

### Setup

1. From the repository root, create a local environment file:

   ```powershell
   Copy-Item .env.compose.example .env.compose
   ```

2. Open `.env.compose` and update any values you need.

   - `POSTGRES_*` configures the database container.
   - `RABBITMQ_DEFAULT_*` configures the message broker.
   - `NEXT_PUBLIC_API_BASE_URL` should remain `http://localhost:5168` for local Docker usage unless you intentionally change the backend port.
   - `NEXT_PUBLIC_AWS_COGNITO_*`, `NEXT_PUBLIC_MAPTILER_API_KEY`, and `S3SETTINGS__*` are optional but required for the related integrations.
   - `AUTHENTICATION__*` values are optional for a local boot, but required for real JWT validation.

3. Start the full stack from the repository root:

   ```powershell
   docker compose --env-file .env.compose up --build
   ```

4. Wait until all services are healthy or started:

   - `rental-app-db` for PostgreSQL
   - `rental-app-mq` for RabbitMQ
   - `rental-app-server` for the API
   - `rental-app-client` for the web app

5. Open the running services:
   - Frontend: `http://localhost:3000`
   - Backend API: `http://localhost:5168`
   - RabbitMQ management UI: `http://localhost:15672`

### Notes

- The backend applies database migrations on startup.
- The frontend reads its public environment variables from the Docker build and runtime configuration.
- If Cognito, MapTiler, or S3 values are omitted, the platform can still start, but the related features may be limited or unavailable.

## Video
