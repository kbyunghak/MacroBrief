# Step 2 Mapping Spec v1 - MacroBrief

## 1. Goal
Define deterministic rule-based mapping from holdings to macro exposure categories for MVP relevance matching.

## 2. Matching Pipeline (Rule-first)

1. Normalize ticker symbol
2. Lookup ticker profile
3. Expand to exposure tags
4. Match incoming news item by category/keywords
5. Score relevance
6. Emit holding-news links

## 3. Category Definitions

- interest_rates
- fed_inflation
- treasury_yields
- oil_energy
- opec
- middle_east_risk
- china_demand
- ai_semiconductors
- consumer_credit
- usd_strength

## 4. Scoring Model (MVP)

- Base category match: +3
- Strong keyword match: +2
- Weak keyword match: +1
- Multi-category reinforcement: +1 per additional category
- Source quality bonus (official/major): +1

Thresholds:
- score >= 4: show in impact cards
- score >= 5: eligible for live alerts

## 5. Initial Ticker Exposure Map

- TSLA: ev, growth, interest_rates, consumer_credit, china_demand, auto_financing
- XOM: oil_energy, opec, middle_east_risk, crude_inventory, usd_strength
- CVX: oil_energy, opec, middle_east_risk, crude_inventory, usd_strength
- NVDA: ai_semiconductors, data_center_spending, export_controls, interest_rates
- SOFI: consumer_credit, interest_rates, fintech, loan_demand, credit_risk

## 6. Keyword Sets (Starter)

interest_rates:
- strong: rate hike, higher rates, policy rate, borrowing costs
- weak: rates, tightening, easing

fed_inflation:
- strong: fed chair, fomc, cpi surprise, inflation print
- weak: fed, inflation, core prices

treasury_yields:
- strong: 10-year yield, treasury auction, yield spike
- weak: treasury yields, bond yields

oil_energy:
- strong: wti surge, brent jumps, crude rally, oil supply shock
- weak: oil prices, energy market, crude

opec:
- strong: opec+ output cut, quota change, production discipline
- weak: opec meeting, opec signals

middle_east_risk:
- strong: strait disruption, regional escalation, supply route risk
- weak: middle east tension, geopolitical risk

china_demand:
- strong: china ev sales drop, china demand shock, weak china imports
- weak: china demand, china growth

ai_semiconductors:
- strong: data center capex, gpu demand, chip export rule
- weak: ai spending, semiconductor outlook

consumer_credit:
- strong: auto loan delinquency, credit tightening, lending standards
- weak: consumer credit, loan demand

usd_strength:
- strong: dollar index surge, strong usd trend
- weak: dollar strength, usd move

## 7. Relevance Explanation Template

Template:
"This update may be relevant to {ticker} because changes in {macro_factor} can affect {exposure_path}."

Examples:
- TSLA / interest_rates: growth valuations and auto financing costs
- XOM / middle_east_risk: crude supply expectations and oil prices
- NVDA / ai_semiconductors: data center spending expectations
- SOFI / consumer_credit: loan demand and credit quality expectations

## 8. Edge Rules

- Deduplicate by normalized headline + source + 6-hour window
- Suppress low-signal repeats from same source within 90 minutes
- If no category score >= 4, keep in background feed only
- Always preserve source URL and publish timestamp

## 9. Step 2 Exit Criteria

Step 2 is complete when:
- Initial ticker exposure map is fixed for MVP symbols
- Category keyword sets are fixed (starter level)
- Scoring thresholds are fixed
- Explanation template is fixed
- JSON config is generated and ready for backend use

---

Version: v1
Date: 2026-05-31
Owner: Andrew
