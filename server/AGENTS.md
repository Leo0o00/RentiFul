# Repository Guidelines

## Project Structure & Module Organization

This repository is the .NET backend for the rental application. The main API host lives in `applications/aggregator/API`, while shared request and response contracts live in `applications/aggregator/SharedContracts`.

Business functionality is organized by module under `modules/`, including `tenant`, `manager`, `property`, `lease`, `payment`, and `application`. Keep new backend features inside the relevant module and follow the existing `Domain`, `Data`, `Features`, and `Contracts` layout.

Integration tests are kept in `*IntegrationTests` projects, such as `modules/tenant/tests/Tenants.IntegrationTests`.

## Build, Test, and Development Commands

- `dotnet build server.sln`: builds the full backend solution.
- `dotnet run --project applications/aggregator/API/API.csproj`: starts the API host locally.
- `dotnet test server.sln`: runs all backend tests.

For Entity Framework Core changes, use the module-specific migration commands documented in `README.md`. Run commands from the `server/` directory unless a command explicitly states otherwise.

## Coding Style & Naming Conventions

Backend code uses C# with nullable reference types and implicit usings enabled. Use PascalCase for types, methods, and public members, camelCase for local variables and parameters, and singular names for domain entities.

Follow the existing feature-folder pattern: `Features/<Area>/<Action>`. Keep module boundaries clear and avoid placing domain-specific logic in the aggregator API unless it is orchestration code.

Prefer concise, explicit names that describe business behavior, for example `GetTenantDetails`, `CreateProperty`, or `RecordPayment`.

## Testing Guidelines

Tests use xUnit with `coverlet.collector`. Add tests to the relevant `*IntegrationTests` project and name test files after the feature under test, for example `GetTenantDetailsTests.cs`.

Run `dotnet test server.sln` before submitting changes. Add or update tests when changing API behavior, persistence logic, module contracts, or feature handlers.

## Commit & Pull Request Guidelines

Git history is minimal and informal, with messages such as `added readme`, `todo added`, and `v0.1.0 - Alpha stage of the application`. Prefer short, imperative commit subjects going forward, for example `server: add tenant update endpoint` or `property: validate listing address`.

Pull requests should include a short summary, affected areas, setup or migration steps, and linked issues when available. Include screenshots only when API changes affect generated docs, UI-facing contracts, or visible client behavior.

## Agent-Specific Instructions

When working in this repository, inspect existing module patterns before adding files. Do not move code across modules unless the change is explicitly architectural. Preserve user changes in the working tree and avoid destructive Git commands unless specifically requested.
