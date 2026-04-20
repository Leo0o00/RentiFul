# Repository Guidelines

## Project Structure & Module Organization
This repository is split into two applications:

- `client/`: Next.js 15 frontend with App Router. Main code lives in `client/src/app`, shared UI in `client/src/components`, state logic in `client/src/state`, utilities in `client/src/lib`, and static assets in `client/public`.
- `server/`: .NET 10 backend. The entry point is `server/applications/aggregator/API`, shared contracts live under `server/applications/aggregator/SharedContracts`, and domain modules are grouped in `server/modules/{tenant|manager|property|lease|payment|application}`.

Keep new backend features inside the relevant module using the existing `Domain`, `Data`, `Features`, and `Contracts` layout.

## Build, Test, and Development Commands
- `cd client && npm run dev`: start the frontend locally on `http://localhost:3000`.
- `cd client && npm run build`: build the production frontend bundle.
- `cd client && npm run lint`: run Next.js and TypeScript ESLint checks.
- `dotnet build server/server.sln`: build the full backend solution.
- `dotnet run --project server/applications/aggregator/API/API.csproj`: start the API host.
- `dotnet test server/server.sln`: run backend tests, including `Tenants.IntegrationTests`.

For EF Core changes, use the module-specific migration commands documented in `server/README.md`.

## Coding Style & Naming Conventions
Frontend code uses TypeScript with strict mode enabled and `@/*` path aliases. Follow the existing style: 2-space indentation, PascalCase for React components (`Navbar.tsx`), camelCase for hooks and helpers (`use-mobile.tsx`, `utils.ts`), and colocated route files under `src/app`.

Backend code uses C# with nullable reference types and implicit usings enabled. Follow the current naming pattern: PascalCase for types and methods, singular domain entities, and feature folders shaped as `Features/<Area>/<Action>`.

## Testing Guidelines
Backend tests use xUnit with `coverlet.collector`. Add tests in `*IntegrationTests` projects and name files after the feature under test, for example `GetTenantDetailsTests.cs`. Run `dotnet test server/server.sln` before opening a PR. The frontend currently has linting but no committed test suite; if you add one, place it near the feature or under `client/src`.

## Commit & Pull Request Guidelines
Git history is minimal and informal (`added readme`, `todo added`, `v0.1.0 - Alpha stage of the application`). Prefer short, imperative commit subjects such as `client: add property filters` or `server: implement tenant update endpoint`.

Pull requests should include a short summary, affected areas (`client`, `server`, or module name), setup or migration steps, and screenshots for UI changes. Link the related issue when one exists.
