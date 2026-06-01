# Step M1 Mobile-Ready API Baseline v1

## Goal
Make API contracts robust for both PWA and future native app clients.

## API Baseline Decisions

1. Versioning
- Use `/api/v1/*`
- Breaking changes only in `/api/v2`

2. Response Envelope
- success response:
  - data
  - meta (optional)
- error response:
  - code
  - message
  - details (optional)
  - traceId

3. Pagination Standard
- Query: `page`, `pageSize`
- Meta: `total`, `page`, `pageSize`, `hasNext`

4. Timestamp Standard
- ISO-8601 UTC only

5. Idempotency
- Add holding endpoint should reject duplicates safely

6. Client Compatibility
- Avoid web-only fields
- Keep payloads neutral for mobile consumption

## Auth Direction (MVP -> Next)
- MVP: temporary anonymous/dev user scope allowed
- Next: token-based auth (JWT or managed provider)
- Plan for per-device session and notification token linkage

## Notification Readiness
- Add future tables/fields for:
  - device_tokens
  - alert_preferences
  - notification_delivery_logs

## Endpoint Priority for Native Reuse
1. holdings CRUD
2. dashboard summary
3. impact cards
4. live alerts
5. macro map
6. relevance feedback

## Exit Criteria
- API response conventions fixed
- Pagination/error/timestamp policy fixed
- Auth migration path defined

---
Version: v1
Date: 2026-05-31
Owner: Andrew
