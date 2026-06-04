-- MacroBrief beta persistence schema
-- Adds storage targets for Step 6 beta validation and AI guardrail audit data.

create extension if not exists pgcrypto;

create table if not exists relevance_feedback (
  id uuid primary key default gen_random_uuid(),
  news_event_id text not null,
  symbol text not null,
  feedback text not null check (feedback in ('relevant', 'not_relevant')),
  created_at timestamptz not null default now()
);

create index if not exists idx_relevance_feedback_created_at
  on relevance_feedback (created_at desc);

create index if not exists idx_relevance_feedback_symbol
  on relevance_feedback (symbol);

create table if not exists kpi_events (
  event_id text primary key,
  event_type text not null check (
    event_type in (
      'app_open',
      'dashboard_refresh',
      'holding_add',
      'holding_remove',
      'impact_feedback',
      'alert_view',
      'source_click'
    )
  ),
  user_id text not null,
  occurred_at timestamptz not null,
  session_id text not null,
  symbol text,
  news_event_id text,
  feedback text check (feedback is null or feedback in ('relevant', 'not_relevant')),
  reason_tag text,
  source_name text,
  source_url text,
  meta jsonb,
  ingested_at timestamptz not null default now()
);

create index if not exists idx_kpi_events_ingested_at
  on kpi_events (ingested_at desc);

create index if not exists idx_kpi_events_occurred_at
  on kpi_events (occurred_at desc);

create index if not exists idx_kpi_events_event_type
  on kpi_events (event_type);

create index if not exists idx_kpi_events_user_id
  on kpi_events (user_id);

create table if not exists ai_explanation_audit (
  id uuid primary key default gen_random_uuid(),
  symbol text not null,
  macro_factor text not null,
  prompt_version text not null,
  output_text text not null,
  validation_failure_codes text[] not null default '{}',
  blocked_terms_detected text[] not null default '{}',
  regeneration_count int not null default 0,
  fallback_used boolean not null default false,
  confidence_label text not null check (confidence_label in ('low', 'medium', 'high')),
  created_at timestamptz not null default now()
);

create index if not exists idx_ai_explanation_audit_created_at
  on ai_explanation_audit (created_at desc);

create index if not exists idx_ai_explanation_audit_symbol
  on ai_explanation_audit (symbol);

create index if not exists idx_ai_explanation_audit_fallback_used
  on ai_explanation_audit (fallback_used);
