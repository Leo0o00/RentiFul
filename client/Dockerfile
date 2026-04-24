FROM oven/bun:1 AS base
WORKDIR /app

FROM base AS deps

COPY package.json bun.lock ./
RUN bun install --frozen-lockfile

FROM base AS builder

ENV NEXT_TELEMETRY_DISABLED=1

ARG NEXT_PUBLIC_API_BASE_URL=""
ARG NEXT_PUBLIC_AWS_COGNITO_USER_POOL_ID=""
ARG NEXT_PUBLIC_AWS_COGNITO_USER_POOL_CLIENT_ID=""
ARG NEXT_PUBLIC_MAPTILER_API_KEY=""
ARG NEXT_PUBLIC_MAPBOX_ACCESS_TOKEN=""

ENV NEXT_PUBLIC_API_BASE_URL=$NEXT_PUBLIC_API_BASE_URL
ENV NEXT_PUBLIC_AWS_COGNITO_USER_POOL_ID=$NEXT_PUBLIC_AWS_COGNITO_USER_POOL_ID
ENV NEXT_PUBLIC_AWS_COGNITO_USER_POOL_CLIENT_ID=$NEXT_PUBLIC_AWS_COGNITO_USER_POOL_CLIENT_ID
ENV NEXT_PUBLIC_MAPTILER_API_KEY=$NEXT_PUBLIC_MAPTILER_API_KEY
ENV NEXT_PUBLIC_MAPBOX_ACCESS_TOKEN=$NEXT_PUBLIC_MAPBOX_ACCESS_TOKEN

COPY --from=deps /app/node_modules ./node_modules
COPY . .

RUN bun run build

FROM node:20-slim AS runner
WORKDIR /app

ENV NODE_ENV=production
ENV NEXT_TELEMETRY_DISABLED=1
ENV HOSTNAME=0.0.0.0
ENV PORT=3000

RUN groupadd --system --gid 1001 nodejs
RUN useradd --system --uid 1001 --gid nodejs nextjs

COPY --from=builder /app/public ./public
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

USER nextjs

EXPOSE 3000

CMD ["node", "server.js"]
