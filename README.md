# MacroBrief

MacroBrief is a portfolio-specific macro-impact dashboard for U.S. stock holdings. It maps same-day macro and breaking-news events to user-selected holdings, then explains why each event may matter using rule-based relevance mapping and AI-assisted safety guardrails.

## Problem

Investors can access endless market headlines, but most news products do not answer the more practical question: "Does this matter to my holdings?" MacroBrief narrows the daily information stream to portfolio-relevant macro context without making investment recommendations.

## Product Positioning

- Information-first dashboard, not investment advice
- Portfolio-specific macro and breaking-news relevance
- Near-real-time alert model with a planned 10-15 minute update cycle
- Rule-based mapping first, AI explanations second
- Source-linked, timestamped context for every alert surface

## MVP Features

- Add and remove holdings/watchlist tickers
- Render a dashboard summary backed by the API
- Show Morning Macro Brief, impact cards, live alerts, and macro map sections
- Map macro categories to holdings exposure paths
- Generate guarded "why it matters" explanations
- Track relevance feedback and source-click events
- Produce beta KPI summaries and weekly rollups
- Audit AI explanation guardrails and fallback behavior

## What MacroBrief Does Not Do

- No buy/sell/hold recommendations
- No price targets
- No stock prediction claims
- No trading execution
- No personalized financial advice

## Architecture

```text
Next.js Web Dashboard
        |
        | REST API
        v
.NET 9 Minimal API
        |
        +-- Holdings service
        +-- Dashboard summary service
        +-- Portfolio insights service
        +-- KPI event service
        +-- Feedback service
        +-- AI explanation guardrail service
        |
        v
In-memory MVP storage + local JSON rule/config files
```

Current implementation uses in-memory services so the project can run locally without provider accounts or secrets. The architecture is intentionally interface-oriented so persistence, news data, market data, and AI providers can be added later without changing the dashboard contract.

## Data Flow

1. User holdings are stored through the holdings API.
2. Mapping rules connect macro categories to sector, ticker, and exposure paths.
3. Dashboard endpoints return summary, impact cards, live alerts, and macro map data.
4. AI explanation text is validated against non-advisory guardrails.
5. Web interactions emit KPI events such as app open, refresh, feedback, alert view, and source click.
6. Weekly rollup summarizes beta signal quality as `green/proceed`, `yellow/iterate`, `red/reposition`, or `insufficient_data`.

## AI Safety Approach

MacroBrief uses deterministic post-generation validation for explanation text. Guardrails reject or regenerate language that includes investment advice, price predictions, price targets, directional calls, or trading instructions. If regeneration fails, the service falls back to a safe rule-based template.

Primary guardrail docs:
- `docs/STEP5_AI_GUARDRAILS_SPEC.md`
- `docs/ai_guardrails.v1.json`
- `docs/PORTFOLIO_SAFETY_COMPLIANCE_SUMMARY.md`

## Tech Stack

- Frontend: React, Next.js, TypeScript
- Backend: .NET 9 Minimal API
- Testing: xUnit, ASP.NET Core test host, Vitest
- Data: in-memory MVP storage; PostgreSQL/Supabase planned
- Contracts: OpenAPI draft and shared JSON schemas
- Delivery path: PWA-first, native-app extensible architecture

## Repository Structure

```text
MacroBrief/
├─ apps/
│  ├─ api/      .NET 9 API, contracts, endpoints, services, tests
│  └─ web/      Next.js dashboard, API client, components, utility tests
├─ docs/        PRD, specs, guardrails, roadmap, portfolio packaging docs
├─ infra/       SQL migration baseline
├─ shared/      shared beta KPI contracts
└─ README.md
```

## Current Status

- Step 0 PRD: complete
- Step 1 dashboard wireframe/spec: complete
- Step 2 mapping rules/spec: complete
- Step 3 ingestion/API spec: complete
- Step 4 dashboard integration: complete
- Step 5 AI guardrails: complete
- Step 6 beta validation: complete
- Step 7 portfolio packaging: complete

Implemented surfaces:
- API and Web MVP scaffolds are connected
- Holdings, dashboard summary, impact cards, live alerts, macro map, relevance feedback, AI guardrail audit, and KPI event endpoints are available
- Web dashboard supports holdings management, refresh, live alert polling, source click tracking, and impact feedback
- API tests and Web utility tests are in place

Known MVP boundaries:
- No persistent database yet
- SQL migration drafts exist under `infra/sql`
- Local JSON persistence is available for personal local data
- No user authentication yet
- No real external news or market data provider connected yet
- No production deployment yet

## Run Locally

Use two terminals: one for the API server and one for the Web server.

Terminal 1 - run the API from the repository root:

```powershell
cd C:\Users\NathanK\Documents\Project\MacroBrief
dotnet run --project apps/api/src/MacroBrief.Api/MacroBrief.Api.csproj
```

Terminal 2 - run the Web app from `apps/web`:

```powershell
cd C:\Users\NathanK\Documents\Project\MacroBrief\apps\web
npm run dev
```

The API runs locally on `http://localhost:5221`. The Web app runs on `http://localhost:3000` and reads the API base URL from `apps/web/.env.local`.

Do not run `npm run dev` from the repository root. The root does not have a `package.json`; the Web app's `package.json` is under `apps/web`.

## Local Data Storage

MacroBrief can persist personal local data without a cloud database. The API reads `apps/api/.env.local` when present.

Recommended local API settings:

```env
MB_STORAGE_MODE=local_json
MB_LOCAL_DATA_DIR=.local-data
```

With this mode enabled, the API stores local JSON files under `.local-data/`:

- `holdings.json`
- `relevance-feedback.json`
- `kpi-events.json`
- `ai-explanation-audit.json`

Both `.env.local` and `.local-data/` are ignored by git, so local personal data and machine-specific settings are not committed. The default storage mode remains `memory` when no local env file is present.

Check the active storage mode while the API is running:

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

## Test

Run API tests:

```powershell
dotnet test apps/api/tests/MacroBrief.Api.Tests/MacroBrief.Api.Tests.csproj
```

Run Web tests:

```powershell
cd apps/web
npm test
```

Latest API test result: 49 passing tests.

## Portfolio Docs

- Current status and roadmap: `docs/PROJECT_STATUS_AND_ROADMAP.md`
- Architecture summary: `docs/PORTFOLIO_ARCHITECTURE_SUMMARY.md`
- Demo walkthrough: `docs/PORTFOLIO_DEMO_WALKTHROUGH.md`
- Safety/compliance summary: `docs/PORTFOLIO_SAFETY_COMPLIANCE_SUMMARY.md`
- Resume/LinkedIn project description: `docs/PORTFOLIO_RESUME_LINKEDIN_DESCRIPTION.md`
- Step 6 handoff/API key readiness: `docs/STEP6_HANDOFF_AND_API_KEY_READINESS.md`

## Future Improvements

- Persistent database for holdings, feedback, KPI events, and AI audit logs
  - beta persistence schema draft: `infra/sql/002_beta_persistence.sql`
- Authentication and per-user data isolation
- Real news and market data provider integration
- Real AI explanation provider behind existing guardrails
- Source deduplication, ranking, and freshness scoring
- Beta KPI monitoring dashboard
- Production deployment and observability
- PWA/mobile readiness from `docs/STEPM0_*` through `docs/STEPM7_*`
