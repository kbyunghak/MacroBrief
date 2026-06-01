# Step 5 AI Guardrails Spec v1 - MacroBrief

## 1. Goal
Define safe, consistent AI output rules for "Why it matters" explanations without producing investment advice.

## 2. AI Scope (Allowed)

AI can:
- Summarize relevant macro/news events briefly
- Explain potential relevance to a holding
- Use conditional, non-predictive language

AI cannot:
- Recommend buy/sell/hold actions
- Predict prices or returns
- Provide target prices
- Use directional calls as advice (bullish/bearish as recommendation)

## 3. Output Format Contract

For each explanation item:
- max_length_chars: 220
- sentence_count: 1 to 2
- required_tone: conditional
- required_phrase_pattern: "may be relevant because"

Output fields:
- symbol
- macro_factor
- explanation_text
- confidence_label (low|medium|high)

## 4. Required Language Policy

Must use wording like:
- "This update may be relevant to {symbol} because..."
- "This development may affect {factor}, which can influence..."

Must avoid wording like:
- "{symbol} will rise/fall"
- "This is a buy/sell"
- "Bullish signal for {symbol}"
- "Guaranteed upside/downside"

## 5. Prompt Policy (System-Level)

### 5.1 Base system prompt
You are generating information-only relevance notes for stock holdings.
Do not provide investment advice, predictions, price targets, or buy/sell/hold recommendations.
Use neutral, conditional language and explain only possible relevance paths.
Always include a source-aware context and keep each explanation concise.

### 5.2 Instruction prompt template
Input:
- holding symbol
- mapped macro categories
- event headline
- source name
- timestamp

Task:
Generate 1-2 concise sentences explaining why the event may be relevant to the holding.
Use non-advisory language and do not predict direction.

Output JSON schema:
{
  "symbol": "string",
  "macro_factor": "string",
  "explanation_text": "string",
  "confidence_label": "low|medium|high"
}

## 6. Validation Rules (Post-Generation)

Reject or regenerate if output contains:
- buy/sell/hold
- price target
- "will go up/down"
- "bullish" or "bearish" as directive
- imperative trading language

If regeneration fails twice:
- fallback to rule-based safe template

## 7. Fallback Templates

Template A:
"This update may be relevant to {symbol} because changes in {macro_factor} can affect {exposure_path}."

Template B:
"This event may matter for {symbol} due to potential impact on {exposure_path}, based on current macro conditions."

## 8. Confidence Label Heuristic

- high: category score >= 6 and strong keyword match present
- medium: score 4-5 with at least one strong or two weak matches
- low: score 3-4 with limited contextual matches

Confidence label is informational only and must not imply expected returns.

## 9. Example Outputs

### 9.1 TSLA + interest_rates
"This update may be relevant to TSLA because higher rate expectations can influence growth-stock valuations and auto financing conditions."

### 9.2 XOM + middle_east_risk
"This event may be relevant to XOM because geopolitical tension can affect crude supply expectations and energy price volatility."

### 9.3 NVDA + ai_semiconductors
"This development may be relevant to NVDA because changes in AI infrastructure spending can influence semiconductor demand expectations."

### 9.4 SOFI + consumer_credit
"This update may be relevant to SOFI because shifts in consumer credit conditions can affect loan demand and credit-risk sentiment."

## 10. Logging & Audit

Store for each generated explanation:
- prompt_version
- output_text
- blocked_terms_detected (if any)
- regeneration_count
- fallback_used (boolean)
- created_at

## 11. Step 5 Exit Criteria

Step 5 is complete when:
- Prompt policy is fixed
- Banned language list is fixed
- Validation/regeneration/fallback flow is fixed
- Example outputs are approved

## 12. Current Implementation Snapshot (2026-06-01)

- Guardrail config source: `docs/ai_guardrails.v1.json`
- API guardrail service scaffold implemented:
  - banned-term detection
  - max-length and required-phrase validation
  - up to 2 regeneration attempts before fallback
  - safe fallback template generation
- Audit log model and in-memory storage implemented:
  - prompt_version
  - output_text
  - blocked_terms_detected
  - regeneration_count
  - fallback_used
  - confidence_label
  - created_at
- Portfolio impact-card reason text now flows through guardrail service.
- Internal audit endpoint added for development validation:
  - `GET /api/v1/internal/ai/audit?limit=20`
- Internal summary endpoint added for quick quality tracking:
  - `GET /api/v1/internal/ai/audit/summary?window=50`
  - includes `fallback_rate` and `fallback_rate_warning` for monitoring

## 13. Step 5 Exit Progress (2026-06-01)

- Prompt policy fixed: Done
- Banned language list fixed: Done
- Validation/regeneration/fallback flow fixed: In progress
  - validation + fallback implemented
  - multi-regeneration strategy implemented (max 2 attempts)
- Example outputs approved: Pending
- Unit/integration tests added for guardrail behavior and audit endpoint.

---

Version: v1
Date: 2026-05-31
Owner: Andrew
