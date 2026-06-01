-- MacroBrief initial schema draft

create table if not exists holdings (
  id uuid primary key,
  user_id uuid not null,
  symbol text not null,
  created_at timestamptz not null default now()
);

create table if not exists news_events (
  id uuid primary key,
  external_id text,
  headline text not null,
  summary text,
  source_name text not null,
  source_url text not null,
  content_url text not null,
  published_at timestamptz not null,
  ingested_at timestamptz not null default now()
);

create table if not exists news_categories (
  id uuid primary key,
  news_event_id uuid not null,
  category_key text not null,
  score int not null
);

create table if not exists holding_relevance (
  id uuid primary key,
  news_event_id uuid not null,
  symbol text not null,
  relevance_score int not null,
  is_live_alert boolean not null default false,
  why_it_matters text
);

create table if not exists ingestion_runs (
  id uuid primary key,
  started_at timestamptz not null,
  ended_at timestamptz,
  source_name text not null,
  fetched_count int not null default 0,
  normalized_count int not null default 0,
  deduped_count int not null default 0,
  failed_count int not null default 0,
  status text not null
);
