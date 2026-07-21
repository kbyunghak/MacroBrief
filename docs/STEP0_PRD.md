# Step 0 PRD v2 - MacroBrief

## 1. Product Summary

MacroBrief is a macro-first dashboard that explains how major market and macro themes may affect a user's U.S. stock holdings.

The product is not a news reader. News and source links support the dashboard, but the primary user experience is a concise portfolio-relevant macro brief.

## 2. Problem

Individual investors can find endless market headlines, but most tools do not answer the practical daily question:

> Which macro changes may matter to my holdings today?

MacroBrief reduces that noise by grouping the day around macro themes first, then showing which holdings are related and why the relationship may matter.

## 3. Target User

Primary user:
- U.S. stock investor with a small to medium watchlist or holdings list
- Tracks macro-sensitive names such as growth, AI, energy, financials, consumer credit, or China-exposed companies
- Wants faster context before market open or during market-moving sessions

Initial portfolio examples:
- `TSLA`
- `NVDA`
- `SOFI`
- `XOM`
- `CVX`

## 4. Product Positioning

MacroBrief is:
- A macro-first portfolio dashboard
- Information-only, not investment advice
- Source-linked and timestamp-aware
- Rule-based first, AI-assisted second
- Local-first during MVP development

MacroBrief is not:
- A general news app
- A brokerage product
- A trading signal product
- A buy/sell recommendation tool
- A price prediction or price-target product

## 5. Product Principles

- Show macro context before individual headlines.
- Connect macro themes to holdings through visible ticker chips.
- Keep source links available for users who want deeper detail.
- Explain relevance without predicting price movement.
- Prefer deterministic mapping rules before AI summarization.
- Keep personal local data private by default during MVP development.

## 6. MVP User Flow

1. User opens MacroBrief.
2. The dashboard shows market signal cards for portfolio mood, futures, yields, oil, and the dollar.
3. The Morning Macro Brief lists the most relevant macro cards.
4. Each macro card shows:
   - macro headline
   - short context
   - "why it matters" explanation
   - related holdings as ticker chips/buttons
   - source link for detail
5. The holdings table shows each ticker and its main related macro theme.
6. Live alerts show portfolio-relevant updates with related ticker chips.
7. The macro map shows how themes connect to holdings.

## 7. MVP Scope

In scope:
- Macro-first mock dashboard prototype
- Market signal strip
- Morning Macro Brief cards
- Related holdings as ticker chips/buttons
- Holdings table with main macro exposure
- Live portfolio alerts
- Macro map
- Source links on macro cards
- Local JSON persistence for personal local data
- Beta monitoring endpoint and dashboard panel
- KPI events, feedback, and guardrail audit infrastructure

Deferred but planned:
- Real news provider integration
- Real market data provider integration
- Real AI explanation provider behind guardrails
- Persistent database for multi-user deployment
- Authentication and per-user data isolation
- Production deployment

Out of scope:
- Buy/sell/hold recommendations
- Price targets
- Stock prediction scores
- Trade execution
- Brokerage integration
- Paid feature gating
- In-app full news reader

## 8. Dashboard Information Architecture

First screen priority:
1. App identity and navigation
2. Information-only disclaimer
3. Market signal strip
4. Morning Macro Brief
5. My Holdings
6. Live Portfolio Alerts
7. Macro Map

Macro card model:
- `title`
- `summary`
- `whyItMatters`
- `relatedHoldings`
- `macroTheme`
- `sourceName`
- `sourceUrl`
- `timestamp`
- `severity`

Ticker chip behavior:
- Chips identify holdings related to a macro theme.
- Chips can become filters or deep links later.
- Chips should not imply a recommendation or directional call.

Source link behavior:
- Source links are the detail path for deeper reading.
- MacroBrief should summarize relevance, not recreate a full news article.

## 9. Data Strategy

Current phase:
- Mock data powers the macro-first Web prototype.
- API-backed components remain available for later integration.
- Personal local data can be stored through local JSON files.
- `.env.local` and `.local-data/` are not committed.

Next data phase:
- Define provider interfaces for news and market data.
- Keep provider keys in local or deployment secrets only.
- Normalize external events into existing macro theme contracts.
- Preserve source URL, source name, and timestamp on every surfaced item.

Database decision:
- A cloud database is not required for single-user local development.
- A database becomes useful for multi-user auth, cross-device sync, hosted beta, event history, deduplication, and monitoring.

## 10. Safety and Compliance Boundaries

MacroBrief must remain information-only.

Allowed language:
- "This may be relevant because..."
- "This can affect..."
- "This theme is related to..."
- "Source context suggests..."

Disallowed language:
- "Buy"
- "Sell"
- "Hold"
- "This stock will rise"
- "This stock will fall"
- "Price target"
- "Guaranteed"
- "Bullish/bearish call" when used as advice

Every generated explanation must pass guardrail validation. If validation fails, the system should use a safe fallback explanation.

## 11. Success Metrics

Prototype validation:
- User understands the macro-first dashboard within the first screen.
- User can identify which macro themes relate to each holding.
- User naturally uses source links for deeper context.
- User does not perceive the app as giving investment advice.

Beta metrics:
- Daily brief open rate
- Alert click-through rate
- Source click rate
- Positive relevance feedback ratio
- False relevance rate
- Weekly active users
- AI fallback rate

## 12. Roadmap Alignment

Completed foundation:
- Step 0 PRD baseline
- Step 1 dashboard wireframe/spec
- Step 2 mapping rules/spec
- Step 3 ingestion/API spec
- Step 4 dashboard integration
- Step 5 AI guardrails
- Step 6 beta validation
- Step 7 portfolio packaging
- Post-Step 7 local-first persistence

Current product direction:
- Step 8: macro-first prototype and documentation alignment

Recommended next steps:
1. Commit and upload the current macro-first prototype baseline.
2. Run local Web smoke testing.
3. Polish responsive dashboard layout.
4. Move mock data into a clean prototype data module.
5. Reconnect selected cards to API-backed data behind feature boundaries.
6. Add real provider integration after the prototype flow is stable.

## 13. Documentation Language Policy

Project documentation should use English as the canonical version.

When Korean documentation is useful, create a sibling file with the `.ko.md` suffix. For example:
- `docs/STEP0_PRD.md`
- `docs/STEP0_PRD.ko.md`

The English document remains the implementation reference. The Korean document should explain the same direction in user-friendly language.

Documentation and language files should be stored as UTF-8.

## 14. Web Language Pack Policy

Web UI copy should be separated from components when the text is part of the product experience.

Current language pack location:
- `apps/web/src/i18n/en.ts`
- `apps/web/src/i18n/ko.ts`
- `apps/web/src/i18n/index.ts`

Current supported locales:
- `en`
- `ko`

Default behavior:
- `en` is the default locale.
- The macro-first dashboard includes a top navigation language switch for `EN` and `KO`.

Commit checkpoint guidance:
- Keep product direction docs and first-screen language packs in the same checkpoint.
- Keep responsive visual polish in a separate checkpoint.
- Keep real provider/API integration in a later checkpoint after the prototype flow is stable.

---

Version: v2
Date: 2026-07-20
Owner: Andrew
