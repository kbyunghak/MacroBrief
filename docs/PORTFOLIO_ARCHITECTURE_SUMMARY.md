# MacroBrief Portfolio Architecture Summary

## One-line Summary

MacroBrief is a full-stack macro-impact dashboard that connects same-day macro events to a user's U.S. stock holdings with rule-based relevance mapping, guarded AI explanations, source-linked alerts, and beta KPI instrumentation.

## System Overview

```text
User
 |
 v
Next.js Dashboard
 |
 v
.NET 9 Minimal API
 |
 +-- Holdings endpoints
 +-- Dashboard summary endpoints
 +-- Impact card endpoints
 +-- Live alert endpoints
 +-- Macro map endpoints
 +-- Feedback endpoints
 +-- KPI event endpoints
 +-- AI audit endpoints
 |
 v
In-memory MVP services
 |
 +-- Mapping rules JSON
 +-- AI guardrail JSON
 +-- Shared KPI contracts
```

## Frontend

The Web app is a Next.js dashboard focused on repeated scanning and quick feedback. It supports holdings management, manual refresh, live alert polling, source click tracking, and impact-card feedback.

Key frontend files:
- `apps/web/app/page.tsx`
- `apps/web/src/components/DashboardClient.tsx`
- `apps/web/src/components/ImpactCardsClient.tsx`
- `apps/web/src/components/LiveAlertsClient.tsx`
- `apps/web/src/lib/api-client.ts`
- `apps/web/src/lib/kpi-events.ts`

## Backend

The API is a .NET 9 Minimal API. Endpoints are grouped by product surface rather than by database table, which keeps the API aligned with dashboard workflows.

Current endpoint groups:
- Holdings CRUD
- Dashboard summary
- Portfolio insights
- Impact cards
- Live alerts
- Macro map
- Relevance feedback
- AI guardrail audit
- KPI event ingestion, summary, and weekly rollup

## Data and Storage

Current MVP storage is in-memory. This keeps the project runnable without external accounts and makes the portfolio demo lightweight.

Current in-memory services:
- `InMemoryHoldingsService`
- `InMemoryPortfolioInsightsService`
- `InMemoryFeedbackService`
- `InMemoryAiExplanationService`
- `InMemoryKpiEventService`

Planned persistence targets:
- holdings
- relevance feedback
- KPI events
- AI explanation audit logs
- normalized news events
- alert sources

## Relevance Mapping

MacroBrief uses deterministic mapping rules to connect macro categories to holdings exposure paths. This rule-first approach keeps relevance explainable before AI enters the flow.

Example mapping flow:
1. Macro event receives a category such as `interest_rates`, `oil_energy`, or `ai_semiconductors`.
2. Mapping rules connect the category to affected sectors, tickers, and exposure paths.
3. The API returns impact cards and alert surfaces with source context.
4. AI explanation text is generated or scaffolded, then validated by guardrails.

## AI Guardrails

AI is used only for conditional explanation text. It is not allowed to recommend trades, predict prices, provide targets, or make directional investment calls.

Validation flow:
1. Candidate explanation is checked for banned patterns.
2. Text must include the required conditional phrase.
3. Text must fit the configured length limit.
4. Invalid text is regenerated up to two times.
5. If regeneration still fails, a safe fallback template is used.
6. Each output is logged for audit review.

## Beta KPI Instrumentation

The Web app emits client-side events into the API:
- `app_open`
- `dashboard_refresh`
- `holding_add`
- `holding_remove`
- `impact_feedback`
- `alert_view`
- `source_click`

The weekly rollup uses feedback sample size and relevance ratio to produce:
- `green/proceed`
- `yellow/iterate`
- `red/reposition`
- `insufficient_data`

## Architecture Tradeoffs

What is intentionally lightweight:
- In-memory services instead of database-backed storage
- Local JSON rules instead of a provider-backed ingestion system
- Scaffolded AI explanation service instead of a paid AI provider

Why this is useful for the current phase:
- The full product flow is demoable without secrets.
- Contracts and boundaries are visible.
- Future provider and persistence work can replace services behind interfaces.
- Safety and KPI validation are testable before production complexity is added.

## Next Architecture Improvements

- Replace in-memory services with database-backed services.
- Add auth and per-user data isolation.
- Connect real news and market data providers.
- Add source deduplication, ranking, and freshness scoring.
- Add real AI provider integration behind the existing guardrails.
- Add production deployment, telemetry, and operational monitoring.
