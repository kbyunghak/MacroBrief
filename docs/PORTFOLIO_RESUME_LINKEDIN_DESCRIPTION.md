# MacroBrief Resume and LinkedIn Description

## Short Resume Version

Built MacroBrief, a portfolio-specific macro-impact dashboard that maps same-day macro and breaking-news events to user-selected U.S. stock holdings using rule-based relevance matching, guarded AI explanations, source-linked alerts, and beta KPI instrumentation.

## Detailed Resume Version

Designed and implemented a full-stack dashboard for U.S. equity investors to reduce macro-news overload by connecting market events to personal holdings. Built a React/Next.js frontend and .NET 9 Minimal API with holdings management, dashboard summaries, impact cards, live alerts, macro map, relevance feedback, AI guardrail audit logs, and KPI event rollups. Defined non-advisory safety constraints, deterministic mapping rules, OpenAPI contracts, beta validation metrics, and a provider-ready architecture for future news, market data, AI, and persistence integrations.

## LinkedIn Project Description

MacroBrief is a portfolio-specific macro-impact dashboard for U.S. stock holdings. The project focuses on a practical information problem: investors see too many headlines, but not enough context about which events may matter to their own portfolio.

The MVP includes a Next.js dashboard, .NET 9 API, holdings workflow, impact cards, live alerts, macro map, relevance feedback, AI explanation guardrails, and beta KPI rollups. The system is intentionally information-first: it avoids buy/sell/hold recommendations, price targets, and prediction claims.

Key engineering themes:
- Rule-based relevance mapping before AI generation
- Guarded AI explanations with validation, regeneration, fallback templates, and audit logs
- Source-linked alert surfaces
- KPI event instrumentation for beta validation
- Provider-ready service boundaries for future persistence, news, market data, and AI integrations

## Interview Storyline

Situation:
Retail investors can access plenty of news but struggle to identify what matters to their own holdings.

Task:
Design a product that maps same-day macro events to a user portfolio while avoiding investment advice.

Action:
- Scoped clear MVP boundaries and non-advisory constraints.
- Designed dashboard information architecture and API contracts.
- Built rule-first relevance mapping.
- Added AI explanation guardrails and fallback templates.
- Added feedback and KPI instrumentation for beta validation.

Result:
MacroBrief is now an end-to-end MVP scaffold with connected web/API surfaces, tested KPI and guardrail behavior, and a clear path toward provider integration, persistence, deployment, and mobile readiness.

## Portfolio Talking Points

- I treated AI as an explanation layer, not a decision engine.
- I separated product relevance from investment advice.
- I designed the MVP so it can run without secrets while still showing production-oriented boundaries.
- I used feedback and weekly KPI rollups to define whether the product should proceed, iterate, reposition, or collect more data.
- I documented the future handoff honestly: persistence, auth, providers, deployment, and mobile readiness are not hidden as completed work.
