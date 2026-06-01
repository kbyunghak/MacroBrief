# Step 9 Execution Plan v1 - MacroBrief

## 1. Goal
Convert specs into implementation-ready work packages with clear priorities and done criteria.

## 2. Sprint Plan (4 Sprints)

### Sprint 1 - API Foundation
Priority: P0
- Initialize .NET Web API project structure
- Implement holdings CRUD endpoints
- Implement dashboard summary mock endpoint
- Add DB connection and migration baseline
Done when:
- Holdings endpoints respond per contract
- Summary endpoint returns valid payload shape

### Sprint 2 - Web Dashboard Shell
Priority: P0
- Initialize Next.js/React app structure
- Build dashboard layout sections from Step 1
- Hook holdings and summary endpoints
- Add loading/empty/error states
Done when:
- User can add/remove holdings from UI
- Dashboard renders summary and holdings reliably

### Sprint 3 - Mapping + Alerts
Priority: P0
- Implement mapping rules loader (docs/mapping_rules.v1.json)
- Implement impact card endpoint
- Implement live alerts endpoint with polling support
- Add macro map endpoint
Done when:
- Impact cards and live alerts render for sample data
- Polling updates work at 10-15 minute config

### Sprint 4 - AI Guardrails + Beta Instrumentation
Priority: P1
- Integrate AI explanation generation pipeline
- Add banned language validation and fallback templates
- Add relevance feedback endpoint and logging
- Add KPI event logging hooks
Done when:
- Explanations pass guardrails
- Feedback pipeline stores events for tuning

## 3. Backlog (Post-MVP)

- Auth and per-user isolation hardening
- Source management admin page
- Alert ranking improvements
- Historical brief archive
- Multi-language support

## 4. Risk Watchlist

- Feed reliability instability
- Relevance precision on mixed-sector holdings
- Cost growth in AI explanation usage
- UX noise from excessive alerts

## 5. Step 9 Exit Criteria

Step 9 is complete when:
- Sprint tasks are defined with done criteria
- P0 vs P1 priorities are fixed
- MVP execution order is locked

---

Version: v1
Date: 2026-05-31
Owner: Andrew
