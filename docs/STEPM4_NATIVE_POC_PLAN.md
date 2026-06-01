# Step M4 Native PoC Plan v1

## Goal
Validate native feasibility with minimal scope before full app build.

## Candidate Stacks
- Option A: React Native + Expo (recommended)
- Option B: Flutter

## PoC Scope (3 screens)
1. Holdings list/add/remove
2. Dashboard summary + top impact card
3. Live alerts list + detail

## PoC Requirements
- Connect to existing `/api/v1` endpoints
- Render timestamps/source links
- Handle loading/empty/error states
- Confirm basic performance on physical device

## Evaluation Criteria
- Dev velocity
- API integration effort
- Push notification readiness
- UI consistency cost vs PWA

## Decision Output
- Choose RN/Expo or Flutter
- Record tradeoffs and timeline impact

## Exit Criteria
- 3-screen native PoC complete
- Stack decision finalized

---
Version: v1
Date: 2026-05-31
Owner: Andrew
