# Frontend Documentation

This project contains the RentiFul frontend, a Next.js 15 application that delivers the public property discovery experience plus authenticated dashboards for tenants and managers.

## Architecture Overview

The frontend uses the App Router and separates the product into route groups:

- `src/app/(nondashboard)`: public pages such as the landing page, property search, and property details.
- `src/app/(dashboard)`: authenticated dashboard experiences for tenants and managers.
- `src/app/(auth)`: authentication provider wiring used by the application shell.

At the application root, `src/app/providers.tsx` composes the main cross-cutting providers:

- Redux store for client-side application state
- RTK Query for server communication and caching
- AWS Amplify `Authenticator.Provider` for authentication context
- A local auth wrapper that connects authenticated sessions to the application

## Project Structure

- `src/app`: route definitions, layouts, and page-level UI
- `src/components`: reusable UI building blocks and shared screens
- `src/components/ui`: lower-level design-system style primitives
- `src/state`: Redux store setup and RTK Query API definitions
- `src/hooks`: custom React hooks
- `src/lib`: shared utilities and helper functions
- `src/types`: shared frontend type definitions
- `public`: static assets

## Core Frontend Flows

### Public Experience

- Landing page sections live under `src/app/(nondashboard)/landing` and introduce the product.
- Search lives under `src/app/(nondashboard)/search` and combines filter controls, listings, and a map view.
- Property detail pages live under `src/app/(nondashboard)/search/[id]` and assemble the overview, media, location, contact widget, and application modal.

### Tenant Dashboard

- Favorites, current residences, applications, and settings live under `src/app/(dashboard)/tenants`.
- Tenant actions are backed by RTK Query endpoints for profile updates, favorites, current residences, leases, payments, and applications.

### Manager Dashboard

- Property creation, property management, manager applications, and settings live under `src/app/(dashboard)/managers`.
- Manager actions are backed by RTK Query endpoints for manager profile data, property creation, manager property lists, and application status updates.

## Data and State Management

The frontend centralizes API communication in `src/state/api.ts` using RTK Query.

Key responsibilities include:

- attaching JWT bearer tokens from the Amplify session to outgoing requests
- resolving the authenticated user and synchronizing them with the backend
- caching property, tenant, manager, lease, payment, and application data
- invalidating cache entries after mutations such as property creation, favorites changes, or application status updates

This keeps the route components mostly focused on rendering and user interactions while shared networking logic stays in one place.

## External Integrations

- AWS Cognito via Amplify for authentication and user session handling
- MapTiler and Leaflet for map rendering in the property discovery flow
- Backend REST endpoints exposed by the .NET API

## Environment Variables

The frontend expects these main environment variables:

- `NEXT_PUBLIC_API_BASE_URL`: base URL for the backend API
- `NEXT_PUBLIC_AWS_COGNITO_USER_POOL_ID`: Cognito user pool identifier
- `NEXT_PUBLIC_AWS_COGNITO_USER_POOL_CLIENT_ID`: Cognito app client identifier
- `NEXT_PUBLIC_MAPTILER_API_KEY`: API key for map tiles
- `NEXT_PUBLIC_MAPBOX_ACCESS_TOKEN`: optional map-related token placeholder present in the example file

For Docker Compose, these values are passed both as build arguments and runtime environment variables.

## Local Development

### Run with Node.js

```bash
npm install
npm run dev
```

### Build for production

```bash
npm run build
npm run start
```

### Lint

```bash
npm run lint
```

## Relationship to the Backend

The frontend is designed around the backend module boundaries:

- `properties` powers public search, property details, and manager property lists
- `tenants` powers tenant profile data, favorites, and current residences
- `managers` powers manager profile data
- `leases` and `payments` support residence and payment-related dashboard flows
- `applications` powers tenant submissions and manager review workflows

That alignment keeps feature ownership clearer across the full stack and makes the API surface easier to navigate.

