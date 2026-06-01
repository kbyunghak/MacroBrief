# Step 8 Implementation Scaffold v1 - MacroBrief

## Goal
Create a practical monorepo-ready skeleton so implementation can begin immediately.

## Structure
- apps/web: frontend app (React/Next.js target)
- apps/api: backend app (.NET Web API target)
- shared/contracts: shared API/data contracts
- infra/sql: database bootstrap scripts

## Initial Build Order
1. API contract-first stubs
2. Web dashboard with mocked data
3. Connect web to API summary/holdings
4. Add ingestion worker module
5. Add persistence and polling jobs

## Done in this step
- Folder scaffolding
- Environment variable templates
- Initial SQL schema draft
- Shared contract references
- Developer runbook

---
Version: v1
Date: 2026-05-31
