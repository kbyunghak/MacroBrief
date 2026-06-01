# Step M3 Notification Architecture Plan v1

## Goal
Prepare a notification architecture that supports both PWA alerts now and native push later.

## Notification Types
- In-app live alert (current MVP)
- Web push (optional phase)
- Native push (future app release)

## Event Model
Each alert event includes:
- event_id
- symbol_list
- category
- headline
- source_url
- created_at
- relevance_score

## User Preference Model
- category toggles
- symbol-level toggles
- quiet hours
- max alerts per day

## Data Model Additions (future)
- device_tokens
- notification_preferences
- notification_deliveries

## Delivery Flow
1. Ingestion maps relevant events
2. Alert scorer marks delivery eligibility
3. Preference filter applies user rules
4. Delivery channel selected (in-app/web/native)
5. Delivery logged for analytics and retries

## Safety Rules
- Keep non-advisory language in notification text
- Include source and timestamp access in detail view

## Exit Criteria
- Notification event schema fixed
- Preference model fixed
- Delivery flow documented

---
Version: v1
Date: 2026-05-31
Owner: Andrew
