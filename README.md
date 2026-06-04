# MacroBrief

MacroBrief is a portfolio-specific macro-impact dashboard for U.S. stock holdings.
It maps same-day macro/breaking-news events to your holdings and explains why each event may matter.

## Product Positioning
- Information-first dashboard (not investment advice)
- Near-real-time portfolio-relevant alerts (10-15 minute update cycle)
- Rule-based relevance mapping + AI-assisted explanation guardrails

## What This App Does
- Store user holdings/watchlist tickers
- Map macro categories to holdings exposure
- Ingest and connect same-day macro/market updates
- Show dashboard sections:
  - My Holdings
  - Morning Macro Brief
  - Holding/Sector Impact Cards
  - Live Portfolio Alerts
  - Macro Map
- Keep source-linked, time-stamped context

## What This App Does Not Do
- No buy/sell/hold recommendations
- No price targets
- No stock prediction claims
- No trading execution

## Current Implementation Status
- Current phase: Step 6 of 7, beta validation instrumentation.
- API and Web MVP scaffolds are implemented and connected.
- Holdings, dashboard summary, impact cards, live alerts, macro map, relevance feedback, AI guardrail audit, and KPI event endpoints are available.
- Next.js dashboard supports holdings management, refresh, live alert polling, source click tracking, and impact feedback.
- Tests exist for API endpoints and Web utility behavior.
- Detailed status and roadmap: `docs/PROJECT_STATUS_AND_ROADMAP.md`

## Tech Stack
- Frontend: React / Next.js
- Backend: .NET 9 Web API
- Testing: xUnit + ASP.NET Core test host + Vitest
- DB: PostgreSQL or Supabase (planned)
- Delivery: PWA-first, native-app extensible architecture

## Repository Structure
```text
MacroBrief/
├─ apps/
│  ├─ api/
│  │  ├─ src/
│  │  │  └─ MacroBrief.Api/
│  │  │     ├─ Contracts/
│  │  │     │  ├─ Dashboard/
│  │  │     │  └─ Holdings/
│  │  │     ├─ Endpoints/
│  │  │     ├─ Models/
│  │  │     ├─ Services/
│  │  │     ├─ Program.cs
│  │  │     └─ MacroBrief.Api.csproj
│  │  └─ tests/
│  │     └─ MacroBrief.Api.Tests/
│  │        ├─ Endpoints/
│  │        ├─ Services/
│  │        └─ MacroBrief.Api.Tests.csproj
│  └─ web/
│     ├─ src/
│     ├─ .env.example
│     └─ README.md
├─ docs/
│  ├─ STEP0_PRD.md
│  ├─ STEP1_WIREFRAME_SPEC.md
│  ├─ STEP2_MAPPING_SPEC.md
│  ├─ STEP3_INGESTION_API_SPEC.md
│  ├─ STEP4_DASHBOARD_INTEGRATION_SPEC.md
│  ├─ STEP5_AI_GUARDRAILS_SPEC.md
│  ├─ STEP6_BETA_VALIDATION_SPEC.md
│  ├─ STEP7_PORTFOLIO_PACKAGING_SPEC.md
│  ├─ STEP8_IMPLEMENTATION_SCAFFOLD.md
│  ├─ STEP9_EXECUTION_PLAN.md
│  ├─ STEPM0_MOBILE_TRANSITION_PRINCIPLES.md
│  ├─ STEPM1_MOBILE_READY_API_BASELINE.md
│  ├─ STEPM2_PWA_MOBILE_UX_CHECKLIST.md
│  ├─ STEPM3_NOTIFICATION_ARCHITECTURE_PLAN.md
│  ├─ STEPM4_NATIVE_POC_PLAN.md
│  ├─ STEPM5_STORE_RELEASE_READINESS.md
│  ├─ STEPM6_MOBILE_BETA_ROLLOUT_PLAN.md
│  └─ STEPM7_LAUNCH_DECISION_FRAMEWORK.md
├─ infra/
│  └─ sql/
│     └─ 001_init.sql
├─ shared/
│  └─ contracts/
└─ README.md
```

## Test Location
API tests are located at:
- `apps/api/tests/MacroBrief.Api.Tests`

Run tests:
```powershell
dotnet test apps/api/tests/MacroBrief.Api.Tests/MacroBrief.Api.Tests.csproj
```

## Architecture & Planning Docs
- Current status and roadmap: `docs/PROJECT_STATUS_AND_ROADMAP.md`
- Product and MVP: `docs/STEP0_PRD.md`
- API/ingestion contracts: `docs/STEP2_MAPPING_SPEC.md`, `docs/STEP3_INGESTION_API_SPEC.md`, `docs/openapi.macrobrief.v1.json`
- AI safety policy: `docs/STEP5_AI_GUARDRAILS_SPEC.md`, `docs/ai_guardrails.v1.json`
- Beta validation: `docs/STEP6_BETA_VALIDATION_SPEC.md`, `docs/STEP6_WEEK1_BASELINE_RUNBOOK.md`
- Mobile expansion plan (M0-M7): `docs/STEPM0_*` to `docs/STEPM7_*`

## Next Implementation Target
- Step 6 - 3/4: harden API KPI tests.
- Step 6 - 4/4: prepare Step 7 handoff and API key readiness docs.
- Step 7: package MacroBrief as a portfolio/interview-ready project artifact.
