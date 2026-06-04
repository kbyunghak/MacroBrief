# MacroBrief Demo Walkthrough

## Goal

Demonstrate MacroBrief as an end-to-end product: holdings input, portfolio-relevant macro context, guarded explanations, source-linked alerts, and beta feedback instrumentation.

Target length: 5-7 minutes.

## Demo Setup

Run API:

```powershell
dotnet run --project apps/api/src/MacroBrief.Api/MacroBrief.Api.csproj
```

Run Web:

```powershell
cd apps/web
npm run dev
```

Open:
- API: `http://localhost:5221`
- Web: `http://localhost:3000`

Suggested sample holdings:
- `NVDA`
- `TSLA`
- `XOM`
- `SOFI`

## Talk Track

### 1. Product Purpose

"MacroBrief is a portfolio-specific macro-impact dashboard. The goal is not to predict stocks or tell users what to buy. The goal is to answer a narrower information question: which same-day macro events may matter to my holdings, and why?"

Point out:
- Information-first positioning
- No recommendations
- Source-linked context

### 2. Holdings Workflow

Add or review sample holdings.

Explain:
- Holdings are the personalization layer.
- MacroBrief uses them to filter and prioritize macro relevance.
- Current MVP stores holdings in memory; persistence is a planned next step.

### 3. Dashboard Summary

Show the Morning Macro Brief and top portfolio context.

Explain:
- The dashboard is organized for scanning.
- It groups broad macro signals before drilling into individual holdings.
- The API-backed dashboard keeps each section isolated and refreshable.

### 4. Impact Cards

Open or point to the holding/sector impact cards.

Explain:
- Mapping rules connect macro categories to exposure paths.
- AI text is used only to explain possible relevance.
- Guardrails prevent advice-like language.

Example framing:
"For NVDA, an AI infrastructure headline maps to semiconductor demand expectations. The explanation stays conditional: this may be relevant because the macro factor can influence an exposure path."

### 5. Live Alerts and Sources

Show live alerts and source links.

Explain:
- Alerts are designed for near-real-time use.
- Source names and URLs are preserved.
- Source clicks are tracked as KPI events for beta validation.

### 6. Feedback Loop

Submit relevant/not-relevant feedback on an impact card.

Explain:
- Feedback is not used to recommend trades.
- It measures mapping quality.
- Weekly rollup uses feedback sample size and relevance ratio to decide whether to proceed, iterate, reposition, or collect more data.

### 7. KPI and Safety Close

Mention the internal KPI and AI audit endpoints.

Useful endpoints:
- `GET /api/v1/internal/events/summary`
- `GET /api/v1/internal/events/weekly-rollup?days=7`
- `GET /api/v1/internal/ai/audit/summary`

Close with:
"The interesting engineering challenge is balancing relevance, safety, and signal quality. MacroBrief keeps the core product useful while staying explicit about what it does not do."

## Interview Questions to Invite

- How would you move from in-memory storage to production persistence?
- How would you evaluate relevance quality during beta?
- How do the AI guardrails avoid investment advice?
- How would real news and market providers plug into the current service boundaries?
- What would you deploy first, and what would remain intentionally out of scope?
