# MacroBrief Project Status and Roadmap

Last updated: 2026-06-03

## 1. Current Progress

Current phase: Step 6 of 7

Step 6 progress:
- 1/4 KPI insufficient data handling: complete
- 2/4 Week 1 baseline runbook: complete
- 3/4 API KPI test hardening: next
- 4/4 Step 7 handoff and API key readiness: pending

Overall status:
- Step 0 PRD: complete
- Step 1 dashboard wireframe/spec: complete
- Step 2 mapping rules/spec: complete
- Step 3 ingestion/API spec: complete
- Step 4 dashboard integration: complete
- Step 5 AI guardrails: complete
- Step 6 beta validation: in progress
- Step 7 portfolio packaging: pending

## 2. What Is Implemented

API:
- .NET 9 Minimal API structure
- Holdings CRUD endpoints
- Dashboard summary endpoint
- Impact cards endpoint
- Portfolio insights endpoints
- Live alerts endpoint with source fields
- Macro map endpoint
- Relevance feedback endpoint
- AI guardrail audit endpoints
- KPI event ingestion endpoints
- KPI summary and weekly rollup endpoints

Web:
- Next.js dashboard app
- API-backed dashboard rendering
- Holdings add/remove flow
- Manual dashboard refresh
- Live Alerts polling and focus refresh
- Impact card feedback actions
- Source click tracking
- Client-side KPI event emitter
- Web unit tests with Vitest

Contracts and docs:
- OpenAPI draft: `docs/openapi.macrobrief.v1.json`
- Mapping rules: `docs/mapping_rules.v1.json`
- AI guardrails: `docs/ai_guardrails.v1.json`
- Beta KPI event contract: `shared/contracts/beta_kpi_events.v1.json`
- Beta weekly rollup contract: `shared/contracts/beta_weekly_rollup.v1.json`
- Week 1 baseline runbook: `docs/STEP6_WEEK1_BASELINE_RUNBOOK.md`

Testing:
- API endpoint tests with xUnit and WebApplicationFactory
- Web utility tests with Vitest
- User confirmed `npm test` passed in `apps/web`

## 3. Current Behavior

Local API:
- Runs on `http://localhost:5221`
- Uses in-memory services for MVP scaffolding
- Stores KPI events only while the API process is running

Local Web:
- Runs on `http://localhost:3000`
- Reads API base URL from `apps/web/.env.local`
- Development command is `npm run dev`
- Production command requires `npm run build` before `npm run start`

KPI rollup:
- `GET /api/v1/internal/events/weekly-rollup?days=7`
- Returns `insufficient_data` until at least 5 relevance feedback events exist in the selected window
- Returns `green/proceed`, `yellow/iterate`, or `red/reposition` once feedback sample size is sufficient

## 4. Important Boundaries

MacroBrief is information-first:
- No buy/sell/hold recommendations
- No price targets
- No stock prediction claims
- No trading execution

Current implementation is still MVP scaffolding:
- No persistent database yet
- No user auth yet
- No real external market/news provider connected yet
- No paid feature gating yet
- No production deployment yet

## 5. API Keys

API keys are not needed for the current in-memory MVP flow.

API keys become relevant in Step 7 handoff or the next implementation phase when we connect real providers:
- News or market data provider key
- AI provider key if replacing the in-memory explanation service
- Database/Supabase credentials when persistence is added
- Deployment secrets when hosting is added

Environment files with real secrets should not be committed:
- Commit `.env.example`
- Do not commit `.env`
- Do not commit `.env.local`
- Do not commit provider keys or deployment secrets

## 6. Next Work

### Step 6 - 3/4: API KPI Test Hardening

Goal:
Lock down KPI event and weekly rollup behavior before moving to portfolio packaging.

Planned tests:
- invalid event type returns 400
- missing required event fields returns 400
- event summary counts event types correctly
- weekly rollup returns `yellow/iterate`
- weekly rollup returns `red/reposition`
- weekly rollup preserves `insufficient_data` for small samples

Done when:
- API tests cover green/yellow/red/insufficient_data decision paths
- event ingestion validation tests exist
- changes are committed

### Step 6 - 4/4: Step 7 Handoff and API Key Readiness

Goal:
Prepare the project for real provider integration without adding secrets.

Planned work:
- list required future provider keys in `.env.example`
- document provider interface plan
- document which services are currently in-memory
- define persistence handoff notes

Done when:
- no real API key is required to run the MVP
- future API key names are documented
- Step 7 packaging can accurately describe current architecture and next improvements

### Step 7: Portfolio Packaging

Goal:
Package MacroBrief as a strong portfolio/interview artifact.

Planned work:
- refresh root README with current architecture
- write architecture summary
- write demo walkthrough script
- write safety/compliance summary
- write resume/LinkedIn project description
- document future improvements honestly

Done when:
- README is current
- demo flow is clear
- project story is interview-ready

## 7. Later Roadmap

Post-Step 7 implementation candidates:
- persistent database for holdings, feedback, and KPI events
- auth and per-user data isolation
- real news/market data ingestion
- real AI explanation provider behind guardrails
- source deduplication and ranking
- beta dashboard for KPI monitoring
- deployment setup
- PWA/mobile readiness from `docs/STEPM0_*` through `docs/STEPM7_*`

## 8. Daily Development Commands

Run API:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief
$env:DOTNET_CLI_HOME='C:\Users\kbyun\Documents\MacroBrief\.dotnet'
dotnet run --project apps/api/src/MacroBrief.Api/MacroBrief.Api.csproj
```

Run Web:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief\apps\web
npm run dev
```

Run API tests:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief
$env:DOTNET_CLI_HOME='C:\Users\kbyun\Documents\MacroBrief\.dotnet'
dotnet test apps/api/tests/MacroBrief.Api.Tests/MacroBrief.Api.Tests.csproj
```

Run Web tests:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief\apps\web
npm test
```
