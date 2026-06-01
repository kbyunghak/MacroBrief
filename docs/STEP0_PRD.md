# Step 0 PRD v1 - MacroBrief

## 1. Product Definition

### 1.1 One-line definition
MacroBrief is a portfolio-specific macro and breaking-news dashboard that maps same-day events to a user's U.S. stock holdings and explains why each event may matter.

### 1.2 Product positioning
- A near-real-time macro-impact dashboard for U.S. stock portfolios
- Information-first product, not a stock recommendation product

### 1.3 Problem statement
U.S. stock investors face information overload. They can find news, but it is hard to quickly identify which same-day macro events are relevant to their own holdings.

### 1.4 Target users
- Primary: U.S. stock investors with watchlists/holdings
- Typical profile: users tracking growth + energy + AI-related names

## 2. Scope

### 2.1 In-scope for MVP
- User enters and stores U.S. stock tickers
- Rule-based macro exposure mapping per ticker
- Same-day macro/market news ingestion (RSS/API/official feeds)
- Relevance mapping from events to holdings
- Dashboard sections:
  - My Holdings
  - Morning Macro Brief
  - Holding/Sector Impact Cards
  - Live Portfolio Alerts
  - Macro Map
- Why-it-matters explanations (AI-assisted)
- Source-linked cards with timestamps
- Information-only disclaimer
- Near-real-time alerts with 10-15 minute update cycle during market hours

### 2.2 Out-of-scope for MVP
- Buy/sell/hold recommendations
- Price targets, prediction scores
- Trading execution
- Native iOS/Android app
- Brokerage account integration
- Advanced charting suite
- Tick-level real-time market data
- Payments/subscription system

## 3. Product Principles

- Explain relevance, do not predict outcomes
- Show source and time for every key item
- Prioritize clarity over quantity
- Start with deterministic rules, then enhance with AI

## 4. UX Requirements (MVP)

### 4.1 Required sections
- My Holdings: editable ticker list
- Morning Macro Brief: daily top macro themes
- Impact Cards: grouped by ticker/sector with related macro tags
- Live Alerts: 10-15 minute updates with related holdings
- Macro Map: macro category to holdings relationship view

### 4.2 Required card fields
- Headline
- Source
- Timestamp
- Related holdings
- Macro tag(s)
- Why it matters explanation

### 4.3 Safety language
Use:
- "This event may be relevant because..."
Avoid:
- "This stock will rise"
- "This is bullish"
- "X is a buy"

## 5. Data and Matching Strategy

### 5.1 Matching architecture
1. Rule-based exposure mapping
2. Keyword/category matching
3. AI-assisted summarization and explanation

### 5.2 Initial macro categories
- Interest Rates
- Fed/Inflation
- Treasury Yields
- Oil/Energy
- OPEC+
- Middle East Risk
- China Demand
- AI/Semiconductors
- Consumer Credit
- USD Strength

### 5.3 Example ticker exposure mapping
- TSLA: EV, growth, rates, consumer credit, China demand, auto financing
- XOM/CVX: oil, energy, OPEC+, Middle East risk, crude inventory, USD
- NVDA: AI, semiconductors, data center capex, export controls, rates
- SOFI: rates, consumer credit, fintech, loan demand, credit risk

## 6. Technical Direction

- Frontend: React or Next.js
- Backend: .NET Web API
- DB: PostgreSQL or Supabase
- Jobs: scheduler/worker for ingestion and alert refresh
- AI: summarization and relevance explanation only
- Delivery: mobile-friendly PWA

## 7. KPIs and Validation

### 7.1 Early product KPIs (beta)
- D7 retention (target: >= 20%)
- Daily brief open rate
- Alert click-through rate
- "Relevant" feedback ratio
- Source click rate

### 7.2 User validation phase
- Beta size: 20-50 users
- Success signal: recurring weekly usage + positive relevance feedback

## 8. Risks and Mitigations

- Data licensing risk
  - Mitigation: start with compliant RSS/official feeds and track source terms
- Low relevance/noise risk
  - Mitigation: strict tag rules + "not relevant" feedback loop
- Legal/compliance language risk
  - Mitigation: non-advisory wording templates + visible disclaimer
- Cost creep risk
  - Mitigation: cap AI usage and prioritize rule-first pipeline

## 9. Delivery Plan (12 weeks)

1. Week 1: finalize product definition + UX wireframe lock
2. Weeks 2-3: implement rule-based exposure map v1
3. Weeks 4-6: build ingestion pipeline + dedupe + storage
4. Weeks 7-8: connect dashboard UI to live data
5. Weeks 9-10: add AI-assisted explanations and quality guardrails
6. Weeks 11-12: beta test (20-50 users), iterate, and package portfolio artifacts

## 10. Step 0 Exit Criteria

Step 0 is complete when:
- Product one-liner and positioning are fixed
- MVP in-scope/out-of-scope is approved
- Update frequency is fixed at 10-15 minutes for MVP live alerts
- Safety language policy is fixed
- 12-week plan and KPIs are approved

---

Version: v1
Date: 2026-05-31
Owner: Andrew
