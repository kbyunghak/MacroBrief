# Step 7 Portfolio Packaging Spec v1 - MacroBrief

## 1. Goal
Package MacroBrief as a strong portfolio project artifact for resume, GitHub, and interviews.

## 2. Packaging Outputs

Required assets:
- Project README
- Architecture summary
- Feature and scope statement
- Safety/compliance statement
- Demo walkthrough script
- Impact and learning summary

## 3. README Structure

1. Project one-liner
2. Problem statement
3. Product positioning
4. Key features (MVP)
5. System architecture (high-level)
6. Data pipeline flow
7. Rule-based mapping + AI explanation approach
8. Safety constraints (no investment advice)
9. Tech stack
10. Roadmap status (Step 0-6 done)
11. Future improvements

## 4. Resume/LinkedIn Description Draft

Short version:
Built a portfolio-specific macro-impact dashboard that maps same-day macro and breaking-news events to user-selected U.S. stock holdings with rule-based relevance matching, AI-assisted explanations, source-linked alerts, and non-advisory safety guardrails.

Detailed version:
Designed a full-stack product concept for U.S. equity investors to reduce daily information overload by connecting macro events to personal holdings. Defined deterministic mapping rules, near-real-time alert flows (10-15 minute updates), AI explanation guardrails, and a dashboard-first PWA architecture using React/Next.js + .NET Web API + PostgreSQL/Supabase.

## 5. Interview Storyline (STAR-style)

Situation:
Retail investors can access plenty of news but struggle to identify what matters to their own holdings.

Task:
Design a product that maps same-day macro events to a user portfolio while avoiding investment advice.

Action:
- Scoped clear MVP boundaries
- Designed dashboard IA and contracts
- Built rule-first relevance framework
- Added AI explanation guardrails and fallback templates
- Defined ingestion/API integration and beta KPIs

Result:
A production-oriented product blueprint ready for implementation, testing with 20-50 users, and iterative quality tuning via relevance feedback.

## 6. Architecture Narrative (Interview-ready)

- Frontend: Single-page PWA dashboard with section-level loading/error isolation
- Backend: .NET API serving summary, impact cards, alerts, and macro map
- Data: Ingestion pipeline normalizes and deduplicates sources every 10-15 minutes
- Relevance: Rule-based category scoring mapped to ticker exposure tags
- AI: Conditional relevance explanations with strict non-advisory validation
- Safety: Explicit disclaimer and banned-language post-processing

## 7. Demo Script (5-7 minutes)

1. Introduce product purpose and non-advisory boundary
2. Add sample holdings (TSLA, XOM, NVDA, SOFI)
3. Show Morning Macro Brief and top exposure summary
4. Open an impact card and explain mapping logic
5. Show live alerts and source links
6. Submit relevant/not-relevant feedback
7. Explain how feedback tunes mapping quality

## 8. Portfolio Quality Checklist

- Clear problem-solution fit
- Scope discipline (what is intentionally excluded)
- End-to-end thinking (data -> logic -> UX)
- Safety/compliance awareness
- Metrics and validation plan
- Practical roadmap and iteration strategy

## 9. Step 7 Exit Criteria

Step 7 is complete when:
- Portfolio messaging is finalized
- README structure is finalized
- Interview storyline is finalized
- Demo walkthrough is finalized

---

Version: v1
Date: 2026-05-31
Owner: Andrew
