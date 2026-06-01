# Step 3 Ingestion & API Spec v1 - MacroBrief

## 1. Goal
Define the MVP data ingestion pipeline and backend API contract required to power the dashboard.

## 2. Pipeline Overview

Scheduler (10-15 min) -> Source Fetch -> Normalize -> Deduplicate -> Classify/Map -> Persist -> API Read

## 3. Source Strategy (MVP)

### 3.1 Source types
- Official feeds (central bank, government, agency releases)
- RSS feeds from compliant financial/macroeconomic publishers
- Optional free news APIs with clear usage terms

### 3.2 Source metadata required
- source_id
- source_name
- source_url
- feed_url
- source_type (official/rss/api)
- enabled (boolean)
- quality_tier (official/major/other)

## 4. Ingestion Steps

### 4.1 Fetch
- Poll each enabled source every 10-15 minutes during market hours
- Retry once on transient failures
- Record fetch status and latency

### 4.2 Normalize
Normalize each item into canonical fields:
- external_id
- headline
- summary (optional)
- published_at
- source_name
- source_url
- content_url
- raw_categories (optional)

### 4.3 Deduplicate
Dedup key:
- normalized_headline + source_name + 6-hour time bucket

Rules:
- keep earliest publish timestamp
- merge aliases into one canonical event

### 4.4 Classification
- Apply category keyword scoring from mapping_rules.v1.json
- Assign one or more macro categories
- Compute relevance score per holding

### 4.5 Explanation generation
- Generate why-it-matters text only for events above threshold
- Use safe template language (non-advisory)

### 4.6 Persist
Store:
- canonical news events
- event-category scores
- event-holding relevance links
- generated explanations
- ingestion logs

## 5. Data Model (Logical)

### 5.1 holdings
- id (uuid)
- user_id (uuid)
- symbol (text)
- created_at (timestamp)

### 5.2 news_events
- id (uuid)
- external_id (text)
- headline (text)
- summary (text, nullable)
- source_name (text)
- source_url (text)
- content_url (text)
- published_at (timestamp)
- ingested_at (timestamp)

### 5.3 news_categories
- id (uuid)
- news_event_id (uuid)
- category_key (text)
- score (int)

### 5.4 holding_relevance
- id (uuid)
- news_event_id (uuid)
- symbol (text)
- relevance_score (int)
- is_live_alert (boolean)
- why_it_matters (text)

### 5.5 ingestion_runs
- id (uuid)
- started_at (timestamp)
- ended_at (timestamp)
- source_name (text)
- fetched_count (int)
- normalized_count (int)
- deduped_count (int)
- failed_count (int)
- status (text)

## 6. API Contract (MVP)

### 6.1 GET /api/v1/dashboard/summary
Returns:
- last_updated_at
- holdings_count
- related_updates_count
- top_exposure_categories[]
- morning_brief[]

### 6.2 GET /api/v1/holdings
Returns:
- holdings[]

### 6.3 POST /api/v1/holdings
Body:
- symbol

Behavior:
- uppercase normalize
- duplicate reject

### 6.4 DELETE /api/v1/holdings/{symbol}
Behavior:
- remove symbol from user watchlist

### 6.5 GET /api/v1/impact-cards
Query:
- symbols (optional CSV)

Returns:
- cards[]:
  - title
  - macro_tags[]
  - updates[]
  - why_it_matters

### 6.6 GET /api/v1/live-alerts
Query:
- limit (default 20)

Returns:
- alerts[]:
  - id
  - headline
  - timestamp
  - related_holdings[]
  - source_name
  - source_url
  - is_new

### 6.7 GET /api/v1/macro-map
Returns:
- categories[]:
  - category_key
  - related_holdings[]

### 6.8 POST /api/v1/feedback/relevance
Body:
- news_event_id
- symbol
- feedback (relevant|not_relevant)

Purpose:
- feed rule tuning backlog

## 7. Operational Rules

- Market-hours polling default: every 10-15 minutes
- Off-hours: optional reduced polling every 60 minutes
- Preserve last successful dashboard payload on partial failures
- Source URL and publish timestamp are mandatory for display items

## 8. Step 3 Exit Criteria

Step 3 is complete when:
- Ingestion stages and dedup rules are fixed
- Logical data model is fixed
- MVP endpoint list and payload contracts are fixed
- Feedback endpoint is defined for relevance tuning

---

Version: v1
Date: 2026-05-31
Owner: Andrew
