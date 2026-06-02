# Step 6 Week 1 Baseline Runbook - MacroBrief

## 1. Purpose

Run Week 1 as a no-tuning baseline. The goal is to collect enough usage and relevance data to understand whether MacroBrief is producing useful, portfolio-aware alerts before changing ranking, mapping, or explanation rules.

## 2. Scope

- Duration: 7 calendar days
- Product surface: MVP dashboard
- Rule changes: none during baseline
- Primary audience: beta users tracking U.S. equities
- Required API mode: local or beta API with KPI events enabled

## 3. Daily Operator Checklist

Run once per beta day:

- Confirm API and Web are reachable.
- Open the dashboard and confirm app_open events are accepted.
- Add or remove one test holding only in a test session, not in beta user data.
- Click at least one live alert and one source link in a test session.
- Submit one Relevant or Not Relevant action in a test session.
- Review recent KPI events for ingestion errors.
- Record the daily rollup snapshot.

## 4. Commands

Start API from the repo root:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief
$env:DOTNET_CLI_HOME='C:\Users\kbyun\Documents\MacroBrief\.dotnet'
dotnet run --project apps/api/src/MacroBrief.Api/MacroBrief.Api.csproj
```

Start Web from the web app folder:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief\apps\web
npm run dev
```

Run Web tests:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief\apps\web
npm test
```

Run API tests:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief
$env:DOTNET_CLI_HOME='C:\Users\kbyun\Documents\MacroBrief\.dotnet'
dotnet test apps/api/tests/MacroBrief.Api.Tests/MacroBrief.Api.Tests.csproj
```

## 5. Endpoints To Check

Recent raw events:

```text
GET http://localhost:5221/api/v1/internal/events?limit=50
```

Event count summary:

```text
GET http://localhost:5221/api/v1/internal/events/summary?window=200
```

Weekly KPI rollup:

```text
GET http://localhost:5221/api/v1/internal/events/weekly-rollup?days=7
```

AI guardrail quality summary:

```text
GET http://localhost:5221/api/v1/internal/ai/audit/summary?window=50
```

## 6. Metrics To Record

Record these once per day:

- cohortSize
- weeklyActiveUsers
- d7RetentionRate
- feedbackSampleSize
- relevancePositiveRatio
- falseRelevanceRate
- alertClickThroughRate
- sourceClickRate
- explanationPolicyViolationRate
- kpiHealth
- recommendation
- topFeedbackThemes

## 7. Interpretation Rules

If `feedbackSampleSize < 5`, treat the result as directional only. The API should return:

- `kpiHealth`: `insufficient_data`
- `recommendation`: `collect_more_data`

If `feedbackSampleSize >= 5`, use these gates:

- `green` / `proceed`: relevance positive ratio >= 70%
- `yellow` / `iterate`: relevance positive ratio >= 50% and < 70%
- `red` / `reposition`: relevance positive ratio < 50%

Quality guardrail:

- explanation policy violation rate should remain 0
- any non-zero policy violation should trigger review before further beta expansion

## 8. Week 1 Rules

Do not change these during Week 1:

- mapping keywords
- sector/category rules
- explanation templates
- alert ranking
- source display behavior

Allowed during Week 1:

- fix broken event ingestion
- fix broken UI actions
- fix missing source links
- fix policy/safety defects
- document user feedback themes

## 9. Daily Snapshot Template

```text
Date:
Operator:
API commit:
Web commit:

cohortSize:
weeklyActiveUsers:
d7RetentionRate:
feedbackSampleSize:
relevancePositiveRatio:
falseRelevanceRate:
alertClickThroughRate:
sourceClickRate:
explanationPolicyViolationRate:
kpiHealth:
recommendation:
topFeedbackThemes:

Notes:
Action items:
```

## 10. Week 1 Exit Checklist

Week 1 baseline is complete when:

- 7 daily snapshots are recorded
- at least 5 relevance feedback events are collected
- source click tracking has been observed
- alert view tracking has been observed
- AI guardrail summary has been reviewed
- no baseline rule changes were made without logging

---

Version: v1
Date: 2026-06-02
Owner: Andrew
