# Repository Guidelines

## Project Structure & Module Organization
This repository has two applications:

- `client/`: Next.js 15 frontend. App Router pages live in `client/src/app`, shared UI in `client/src/components`, state in `client/src/state`, utilities in `client/src/lib`, hooks in `client/src/hooks`, and static assets in `client/public`.
- `server/`: .NET backend. The API entry point is `server/applications/aggregator/API`, shared contracts are under `server/applications/aggregator/SharedContracts`, and business modules live in `server/modules/{tenant|manager|property|lease|payment|application}`.

Keep backend changes inside the relevant module using the existing `Domain`, `Data`, `Features`, and `Contracts` layout.

## Build, Test, and Development Commands
- `cd client && npm run dev`: start the frontend at `http://localhost:3000`.
- `cd client && npm run build`: create the production Next.js build.
- `cd client && npm run lint`: run ESLint and TypeScript checks for the frontend.
- `dotnet build server/server.sln`: build the backend solution.
- `dotnet run --project server/applications/aggregator/API/API.csproj`: run the API host locally.
- `dotnet test server/server.sln`: run backend tests, including integration coverage.

For EF Core changes, use the module-specific commands documented in `server/README.md`.

## Coding Style & Naming Conventions
Use TypeScript with strict mode on the frontend and C# with nullable reference types on the backend. Follow the existing 2-space indentation in frontend files. Use PascalCase for React components and C# types, camelCase for hooks and helpers, and keep route files colocated under `src/app`. Use `@/*` path aliases in frontend imports where applicable.

## Testing Guidelines
Backend tests use xUnit with `coverlet.collector`. Add tests in the appropriate `*IntegrationTests` project and name files after the feature under test, for example `GetTenantDetailsTests.cs`. Run `dotnet test server/server.sln` before opening a PR. The frontend currently relies on `npm run lint`; if you add UI tests, keep them near the feature or under `client/src`.

## Commit & Pull Request Guidelines
Recent history is informal, so prefer short, imperative commit subjects such as `client: add property filters` or `server: implement tenant update endpoint`. PRs should include a brief summary, affected areas, setup or migration steps, linked issues when available, and screenshots for UI changes.

## Documentation Lookup
When a task involves a library, framework, SDK, API, CLI, or cloud service, use the `ctx7` CLI to fetch current documentation before answering or changing code.
