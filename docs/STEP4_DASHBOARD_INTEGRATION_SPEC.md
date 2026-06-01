# Step 4 Dashboard Integration Spec v1 - MacroBrief

## 1. Goal
Define how frontend dashboard sections consume Step 3 APIs and render stable MVP behavior.

## 2. Integration Order

1. Load holdings
2. Load summary (morning brief + counters)
3. Load impact cards
4. Load live alerts
5. Load macro map

All calls can run in parallel after holdings are available, except where symbols filtering is required.

## 3. UI-to-API Mapping

### 3.1 Header
- API: GET /api/v1/dashboard/summary
- Fields:
  - holdings_count
  - related_updates_count
  - last_updated_at

### 3.2 My Holdings
- API: GET /api/v1/holdings
- Actions:
  - Add: POST /api/v1/holdings
  - Remove: DELETE /api/v1/holdings/{symbol}

### 3.3 Morning Macro Brief
- API: GET /api/v1/dashboard/summary
- Field: morning_brief[]

### 3.4 Impact Cards
- API: GET /api/v1/impact-cards
- Query: symbols (optional)

### 3.5 Live Alerts
- API: GET /api/v1/live-alerts?limit=20
- Update cadence: 10-15 min polling during market hours

### 3.6 Macro Map
- API: GET /api/v1/macro-map

## 4. Client State Model

### 4.1 Core state
- holdings: Holding[]
- summary: DashboardSummary
- impactCards: ImpactCard[]
- liveAlerts: LiveAlert[]
- macroMap: MacroCategoryLink[]

### 4.2 UI status flags
- isInitialLoading
- isSectionLoading (per section)
- hasPartialError
- lastSuccessfulSyncAt

## 5. Error Handling Rules

- If one section fails, render others with partial warning
- Show retry action per failed section
- Keep previous successful data in-memory until refresh succeeds
- If holdings API fails, block dashboard and show recoverable error

## 6. Polling Rules

- Live alerts polling: every 10-15 min in market hours
- If tab hidden, pause polling and resume on focus
- Manual refresh triggers all section refetch with debounce/cooldown

## 7. Feedback Capture

- In alert detail or card action:
  - Relevant
  - Not relevant
- API: POST /api/v1/feedback/relevance
- Minimal payload:
  - news_event_id
  - symbol
  - feedback

## 8. Acceptance Criteria (Step 4 Exit)

Step 4 is complete when:
- Every dashboard section is mapped to a concrete API endpoint
- Loading/empty/error states are defined per section
- Polling and refresh behavior are fixed
- Feedback action path is fixed

## 9. Current Implementation Snapshot (2026-06-01)

- Web app scaffolded with Next.js App Router at `apps/web`
- API client layer implemented for all Step 3 endpoints
- Dashboard sections wired:
  - Summary
  - Holdings (add/remove)
  - Morning Brief
  - Impact Cards
  - Live Alerts
  - Macro Map
- Live alerts polling wired with configurable interval from `NEXT_PUBLIC_ALERT_POLL_MINUTES`
- Live alerts polling pauses on hidden tab and refreshes on focus return
- Manual refresh action added with partial-failure messaging and 5-second cooldown
- Feedback action (`Relevant` / `Not Relevant`) calls `POST /api/v1/feedback/relevance`
- Partial failure handling implemented with previous-data retention
- Web unit test entry added for dashboard formatting utilities (`vitest`)

## 10. Step 4 Exit Readiness (2026-06-01)

Status: Ready

- Section-to-API wiring is complete for all MVP dashboard sections.
- Section-level loading/empty/error handling is in place.
- Live alerts polling behavior is implemented, including hidden-tab pause and focus refresh.
- Manual refresh behavior is implemented with partial-failure notice and cooldown.
- Feedback action path is wired from UI to API.
- Web unit test scaffold is active and passing (`npm test`).

## 11. Step 5 Entry Checklist

- Keep non-advisory tone and guardrail language consistent in all generated/explained text.
- Introduce explanation validation/fallback pipeline in API before production-facing AI output.
- Add audit-friendly logging fields for explanation generation and fallback reasons.

---

Version: v1
Date: 2026-05-31
Owner: Andrew
