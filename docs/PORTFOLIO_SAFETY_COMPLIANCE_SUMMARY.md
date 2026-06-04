# MacroBrief Safety and Compliance Summary

## Positioning

MacroBrief is an information product. It helps users understand why macro events may be relevant to their holdings, but it does not recommend trades, predict prices, or provide personalized financial advice.

## Explicit Product Boundaries

MacroBrief does not provide:
- Buy, sell, or hold recommendations
- Price targets
- Return predictions
- Guaranteed outcomes
- Trading execution
- Personalized financial advice

## Explanation Policy

Explanations must use conditional language. They should explain possible relevance paths, not expected investment outcomes.

Allowed framing:
- "This update may be relevant because..."
- "This development may affect..."
- "Changes in this macro factor can influence..."

Disallowed framing:
- "This is a buy signal"
- "The stock will go up"
- "Bullish signal"
- "Price target"
- "Guaranteed upside"

## Guardrail Validation

The AI explanation service applies deterministic validation after candidate text is generated.

Validation checks:
- banned language patterns
- required conditional phrase
- max text length

If validation fails:
1. The service attempts regeneration.
2. If regeneration does not satisfy the rules after the allowed attempts, a safe fallback template is used.
3. The output is logged for audit review.

## Audit Fields

Each guarded explanation audit record includes:
- symbol
- macro factor
- prompt version
- output text
- validation failure codes
- blocked terms detected
- regeneration count
- fallback used
- confidence label
- created timestamp

## KPI Safety

Beta KPI events measure product quality and user interaction, not investment performance.

Tracked events include:
- app open
- dashboard refresh
- holding add/remove
- impact feedback
- alert view
- source click

Weekly rollup evaluates relevance feedback and interaction signals. It does not evaluate whether a trade or price prediction was correct.

## Current Limitations

- No production compliance review has been completed.
- No user authentication or per-user data isolation exists yet.
- No production privacy policy is included yet.
- Real provider integrations should be reviewed for data licensing and attribution requirements.

## Production Readiness Additions

Before production launch:
- Add visible non-advisory disclaimer in the deployed app.
- Add auth and per-user data isolation.
- Add durable audit-log storage.
- Review provider licensing and source attribution requirements.
- Add privacy policy and terms of use.
- Add monitoring for fallback rate, blocked terms, and missing source data.
