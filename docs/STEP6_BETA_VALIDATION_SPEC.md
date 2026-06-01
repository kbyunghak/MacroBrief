# Step 6 Beta Validation Spec v1 - MacroBrief

## 1. Goal
Define the beta test plan, success metrics, and feedback loop to validate product relevance and retention before any paid features.

## 2. Beta Scope

- Audience size: 20-50 users
- Duration: 4 weeks
- Market focus: U.S. stock investors
- Product surface: MVP dashboard only

## 3. Beta User Profile

Target participants:
- Users who actively track U.S. equities
- Users with 5-20 holdings/watchlist names
- Users who feel daily macro/news overload

Suggested sample mix:
- Growth-heavy users
- Energy-heavy users
- Mixed portfolios (growth + energy + financials)

## 4. Test Scenarios

### 4.1 Daily brief usage
- User opens Morning Macro Brief at least 3 days/week
- User checks whether summary reflects their holdings

### 4.2 Relevance check
- User opens impact cards and evaluates "Why it matters"
- User submits Relevant/Not relevant feedback

### 4.3 Alert usefulness
- User reviews live alerts during market hours
- User checks source links and related holdings accuracy

### 4.4 Holdings management
- User adds/removes tickers
- User validates refresh quality after holdings changes

## 5. KPI Framework

### 5.1 Core KPIs
- D7 retention: target >= 20%
- Weekly active usage: target >= 30% of beta users
- Alert click-through rate: track baseline and trend
- Source click rate: target directional increase week-over-week
- Relevance positive ratio: target >= 70%

### 5.2 Quality KPIs
- False relevance rate (user marks not relevant)
- Duplicate alert rate
- Missing-source rate (should be 0 for displayed items)
- Explanation policy violation rate (should be 0)

## 6. Feedback Collection Plan

### 6.1 In-product feedback
- Per item: Relevant / Not relevant
- Optional reason tags:
  - wrong category
  - weak explanation
  - stale update
  - duplicate content

### 6.2 Weekly short survey
Questions:
- Did this save you time this week?
- Were alerts relevant to your holdings?
- What felt noisy or missing?
- Would you continue using this weekly?

## 7. Experiment Cadence

- Week 1: Baseline measurement (no rule changes)
- Week 2: Tune category keywords and thresholds
- Week 3: Tune explanation templates and ranking
- Week 4: Stabilization and final KPI readout

Rule changes should be versioned and logged by date.

## 8. Decision Gates

### 8.1 Continue gate
Proceed to broader release prep if:
- D7 retention >= 20%
- Relevance positive ratio >= 70%
- No major safety/policy issues

### 8.2 Iterate gate
Run another beta cycle if:
- Relevance positive ratio is 50-69%
- Duplicate/noise issues remain high

### 8.3 Stop or reposition gate
If relevance positive ratio < 50% after iteration, revisit mapping scope and user segment.

## 9. Deliverables at End of Step 6

- KPI dashboard snapshot
- Top 10 feedback themes
- Rule changes log
- Before/after relevance metrics
- Recommendation: proceed / iterate / reposition

## 10. Step 6 Exit Criteria

Step 6 is complete when:
- Beta plan and KPIs are fixed
- Feedback capture model is fixed
- Decision gates are agreed
- 4-week execution cadence is defined

---

Version: v1
Date: 2026-05-31
Owner: Andrew
