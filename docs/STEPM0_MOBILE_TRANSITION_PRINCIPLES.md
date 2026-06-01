# Step M0 Mobile Transition Principles v1

## Goal
Keep MVP speed with PWA while preserving a clean path to native app store release.

## Core Principle
Build once at domain/API level, adapt per client UI (web PWA now, native later).

## Rules
1. API-first contract (`/api/v1`) for all business flows
2. No client-specific business logic in UI layer
3. Shared naming and payload schema across clients
4. Alert/event model designed for future push delivery
5. Safety policy (non-advisory language) enforced server-side

## Architecture Direction
- Current client: Web PWA
- Future clients: React Native or Flutter
- Shared backend: .NET API + DB + worker jobs
- Shared rules: mapping + guardrails JSON configs

## Data Ownership
- Relevance scoring, mapping, and explanations are backend-owned
- Client only renders server responses

## Exit Criteria
- Mobile transition principles are fixed and documented
- API-first constraints are accepted for new features

---
Version: v1
Date: 2026-05-31
Owner: Andrew
