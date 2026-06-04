# Step 6 Handoff and API Key Readiness - MacroBrief

## Goal

Prepare MacroBrief for Step 7 portfolio packaging and future provider integration without requiring real secrets for the current MVP.

## Current Runtime Boundary

The current MVP runs with in-memory services and does not require provider API keys.

In-memory services:
- `InMemoryHoldingsService`
- `InMemoryPortfolioInsightsService`
- `InMemoryFeedbackService`
- `InMemoryAiExplanationService`
- `InMemoryKpiEventService`

Static/local providers:
- `JsonMappingRulesProvider` loads mapping rules from `docs/mapping_rules.v1.json`
- API contracts are documented in `docs/openapi.macrobrief.v1.json`
- Beta KPI event contracts are stored under `shared/contracts`

## Future Environment Variables

The API placeholder keys are documented in `apps/api/.env.example`.

Expected future variables:
- `MB_NEWS_PROVIDER_API_KEY`
- `MB_MARKET_DATA_API_KEY`
- `MB_AI_PROVIDER_API_KEY`
- `MB_DB_CONNECTION`
- `MB_SUPABASE_URL`
- `MB_SUPABASE_SERVICE_ROLE_KEY`
- `MB_ALLOWED_ORIGINS`
- `MB_ALERT_POLL_MINUTES`

No real keys should be committed.

## Provider Interface Plan

Future provider integration should replace in-memory services behind interfaces instead of changing endpoint contracts.

Recommended interfaces:
- News ingestion provider: normalize provider articles/events into the existing KPI and alert pipeline.
- Market data provider: enrich symbols, sectors, and same-day market context.
- AI explanation provider: generate explanations behind `IAiExplanationService` and preserve guardrail validation.
- Persistence provider: replace in-memory holdings, feedback, and KPI event storage with database-backed services.

## Persistence Handoff Notes

Priority tables:
- holdings
- relevance_feedback
- kpi_events
- ai_explanation_audit
- normalized_news_events
- alert_sources

Migration baseline:
- Start from `infra/sql/001_init.sql`
- Apply `infra/sql/002_beta_persistence.sql` for feedback, KPI event, and AI audit storage.
- Preserve current API response shapes while replacing storage implementation.
- Add per-user isolation before production deployment.

## Step 6 Exit Criteria

Step 6 is complete when:
- KPI event validation tests cover invalid and missing required fields.
- Weekly rollup tests cover `green`, `yellow`, `red`, and `insufficient_data`.
- Future API key names are documented without adding real secrets.
- Current in-memory services and persistence handoff notes are documented.

---

Version: v1
Date: 2026-06-03
