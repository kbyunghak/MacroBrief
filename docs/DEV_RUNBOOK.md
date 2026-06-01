# Dev Runbook (Scaffold Phase)

## 1) Read specs in order
1. docs/STEP0_PRD.md
2. docs/STEP1_WIREFRAME_SPEC.md
3. docs/STEP2_MAPPING_SPEC.md
4. docs/STEP3_INGESTION_API_SPEC.md
5. docs/STEP4_DASHBOARD_INTEGRATION_SPEC.md
6. docs/STEP5_AI_GUARDRAILS_SPEC.md
7. docs/STEP6_BETA_VALIDATION_SPEC.md
8. docs/STEP7_PORTFOLIO_PACKAGING_SPEC.md
9. docs/STEP8_IMPLEMENTATION_SCAFFOLD.md

## 2) Implementation sequence
- Create .NET API skeleton in `apps/api`
- Implement holdings endpoints first
- Create web dashboard shell in `apps/web`
- Connect summary + holdings + alerts
- Add ingestion worker

## 3) Safety checks
- Keep non-advisory disclaimer visible
- Validate AI output against banned terms
- Preserve source URL and timestamp on all cards
