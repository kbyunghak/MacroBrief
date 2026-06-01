# MacroBrief

A portfolio-specific macro-impact dashboard for U.S. stock holdings.

## What it is
MacroBrief maps same-day macro and breaking-news events to user-selected holdings and explains why each event may be relevant.

## What it is not
- Not an investment advisory app
- No buy/sell/hold recommendations
- No price targets or prediction calls

## MVP Features
- Holdings input and storage
- Rule-based exposure mapping
- Same-day news ingestion and dedup
- Holding/sector impact cards
- Live alerts (10-15 min update cycle)
- Macro category-to-holdings map
- AI-assisted "Why it matters" explanations with safety guardrails
- Source-linked updates and visible disclaimer

## Tech Direction
- Frontend: React or Next.js
- Backend: .NET Web API
- Database: PostgreSQL or Supabase
- Jobs: scheduler/worker for periodic ingestion
- Delivery: mobile-friendly PWA

## Project Docs
- docs/STEP0_PRD.md
- docs/STEP1_WIREFRAME_SPEC.md
- docs/STEP2_MAPPING_SPEC.md
- docs/STEP3_INGESTION_API_SPEC.md
- docs/STEP4_DASHBOARD_INTEGRATION_SPEC.md
- docs/STEP5_AI_GUARDRAILS_SPEC.md
- docs/STEP6_BETA_VALIDATION_SPEC.md
- docs/STEP7_PORTFOLIO_PACKAGING_SPEC.md

## Current Status
Planning/specification phase complete through Step 7.

## Next Steps
- Step 8: implementation scaffolding
- Step 9: beta execution and KPI tracking
