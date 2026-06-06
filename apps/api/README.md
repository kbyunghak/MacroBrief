# MacroBrief API

## Purpose

Serve MacroBrief holdings, dashboard summary, impact cards, live alerts, macro map, feedback, AI guardrail audit, and beta KPI event endpoints.

## Stack

- .NET 9 Minimal API
- In-memory MVP services
- PostgreSQL/Supabase planned for persistence

## Contract

Use `docs/openapi.macrobrief.v1.json` as the API contract baseline.

## Current Services

The current MVP does not require external provider keys. By default, these services are in-memory:

- Holdings
- Portfolio insights
- Relevance feedback
- AI explanation/audit
- KPI events

Mapping rules are loaded from `docs/mapping_rules.v1.json`.

## Local JSON Persistence

For local personal data persistence without a cloud database, create `apps/api/.env.local`:

```env
MB_STORAGE_MODE=local_json
MB_LOCAL_DATA_DIR=.local-data
```

This stores holdings, relevance feedback, KPI events, and AI explanation audit logs as JSON files under `.local-data/`. The folder is ignored by git.

Check the active storage mode:

```powershell
Invoke-RestMethod http://localhost:5221/api/v1/internal/storage
```

Check local data file status:

```powershell
Invoke-RestMethod http://localhost:5221/api/v1/internal/local-data/export
```

Reset managed local data files:

```powershell
Invoke-RestMethod -Method Post http://localhost:5221/api/v1/internal/local-data/reset
```

## API Key Readiness

Future provider placeholders are listed in `apps/api/.env.example`:

- News provider key
- Market data provider key
- AI provider key
- Database/Supabase credentials

Do not commit real secrets.
