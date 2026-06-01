# Step 1 Wireframe Spec v1 - MacroBrief

## 1. Goal
Define the MVP screen structure and interaction rules so frontend implementation can start without ambiguity.

## 2. Screen Map (MVP)

1. Onboarding / Holdings Setup
2. Main Dashboard
3. Holdings Edit Panel (modal or side panel)
4. Alert Detail Drawer (optional lightweight)
5. Empty/Error States

## 3. Information Architecture

### 3.1 Top-level layout
- Header
  - Product name: MacroBrief
  - Last updated timestamp
  - Tracked holdings count
  - Related updates count
- Left column
  - My Holdings list
  - Add/Edit holdings action
- Main content
  - Morning Macro Brief card
  - Holding/Sector Impact cards
  - Live Portfolio Alerts card
  - Macro Map card
- Footer
  - Disclaimer: Information only. Not investment advice.

### 3.2 Navigation model
- Single-page dashboard for MVP
- In-page anchor/scroll sections allowed
- No multi-route complexity required in first version

## 4. Screen Specs

### 4.1 Screen A - Onboarding / Holdings Setup
Purpose:
- Collect initial ticker list (5-10 tickers recommended)

Components:
- Intro text: what the app does and does not do
- Input field with ticker validation
- Add button
- Holdings preview chips/list
- Continue button

Validation rules:
- Uppercase ticker symbols only
- Duplicate ticker blocked
- Max 30 tickers for MVP
- Empty state blocks continue

Primary actions:
- Add ticker
- Remove ticker
- Continue to dashboard

### 4.2 Screen B - Main Dashboard
Purpose:
- Show today's macro relevance for the user's holdings

Sections:
1. My Holdings
- Ticker rows with small icon and quick-open arrow
- Optional exposure tag count per ticker

2. Morning Macro Brief
- 3-6 bullet points max
- Single summary line: "Your portfolio is most exposed to ..."

3. Holding/Sector Impact Cards
- Card title format: "TSLA / EV / Growth Stocks"
- Macro tags (chips)
- 3-5 relevant updates
- Why it matters block
- Source link per update

4. Live Portfolio Alerts
- Time-sorted list (newest first)
- Item fields: time, title, related holdings, source
- "NEW" badge for unseen alerts

5. Macro Map
- Category -> holdings mapping list
- Example: Interest Rates -> TSLA, SOFI, NVDA

### 4.3 Screen C - Holdings Edit Panel
Purpose:
- Let user manage watchlist without leaving dashboard

Components:
- Current holdings list
- Add ticker input
- Remove action
- Save/Cancel

Behavior:
- On save, trigger remapping/reload
- Show loading state while refresh runs

### 4.4 Screen D - Alert Detail Drawer (Optional MVP-lite)
Purpose:
- Expand an alert without route change

Fields:
- Full headline
- Source and publish time
- Related holdings
- Macro tags
- Why it matters
- Open source button
- Feedback: Relevant / Not relevant

## 5. Global States

### 5.1 Loading states
- Skeleton cards for all major dashboard sections
- Section-level loader for live alerts refresh

### 5.2 Empty states
- No holdings: prompt to add tickers
- No matching updates: "No relevant updates yet"
- No morning brief: show fallback macro summary unavailable

### 5.3 Error states
- Source fetch error: retry action
- Partial data warning when some feeds fail
- Keep last successful data when refresh fails

## 6. Design/Content Constraints

- Must include visible disclaimer in footer and onboarding
- Avoid recommendation language (buy/sell/target/prediction)
- Why-it-matters tone must be conditional: "may be relevant because"
- Keep each card concise to reduce cognitive load

## 7. Data Contract (UI-facing)

### 7.1 Holding
- symbol: string
- displayName: string (optional)
- exposureTags: string[]

### 7.2 MacroBriefItem
- id: string
- text: string
- category: string
- source: string
- timestamp: ISO string

### 7.3 ImpactUpdate
- id: string
- headline: string
- sourceName: string
- sourceUrl: string
- timestamp: ISO string
- relatedHoldings: string[]
- macroTags: string[]
- whyItMatters: string

### 7.4 LiveAlert
- id: string
- headline: string
- timestamp: ISO string
- relatedHoldings: string[]
- sourceName: string
- sourceUrl: string
- isNew: boolean

## 8. Interaction Rules

- Live alerts refresh every 10-15 minutes during market hours
- Manual refresh button allowed (cooldown recommended)
- After holdings update, remap and re-render impacted sections
- New alerts flagged until viewed in current session

## 9. Acceptance Criteria (Step 1 Exit)

Step 1 is complete when:
- MVP screen list is fixed
- Each section has defined fields and states
- Holdings input/edit behavior is fixed
- Alert update cadence is fixed (10-15 min)
- Safety/disclaimer placement is fixed

---

Version: v1
Date: 2026-05-31
Owner: Andrew
