export type ApiMeta = {
  timestampUtc: string;
};

export type ApiError = {
  code: string;
  message: string;
};

export type ApiResponse<T> = {
  data: T | null;
  error: ApiError | null;
  meta: ApiMeta;
};

export type Holding = {
  symbol: string;
};

export type DashboardSummary = {
  lastUpdatedAt: string;
  holdingsCount: number;
  relatedUpdatesCount: number;
  topExposureCategories: string[];
  morningBrief: string[];
};

export type ImpactCard = {
  symbol: string;
  headline: string;
  impactLevel: string;
  reason: string;
};

export type LiveAlert = {
  id: string;
  createdAtUtc: string;
  category: string;
  severity: string;
  message: string;
  sourceName: string;
  sourceUrl: string;
};

export type MacroMapItem = {
  category: string;
  relatedHoldingsCount: number;
  trendDirection: string;
};

export type RelevanceFeedbackRequest = {
  newsEventId: string;
  symbol: string;
  feedback: "relevant" | "not_relevant";
};

export type KpiEventType =
  | "app_open"
  | "dashboard_refresh"
  | "holding_add"
  | "holding_remove"
  | "impact_feedback"
  | "alert_view"
  | "source_click";

export type KpiEventRequest = {
  eventId: string;
  eventType: KpiEventType;
  userId: string;
  occurredAtUtc: string;
  sessionId: string;
  symbol?: string;
  newsEventId?: string;
  feedback?: string;
  reasonTag?: string;
  sourceName?: string;
  sourceUrl?: string;
  meta?: Record<string, string>;
};

export type StorageStatus = {
  mode: string;
  localDataDirectory: string | null;
  localDataDirectoryExists: boolean;
};

export type KpiEventsSummary = {
  totalEvents: number;
  eventTypeCounts: Record<string, number>;
  latestOccurredAtUtc: string | null;
};

export type BetaWeeklyRollup = {
  weekStartDate: string;
  cohortSize: number;
  weeklyActiveUsers: number;
  d7RetentionRate: number;
  feedbackSampleSize: number;
  relevancePositiveRatio: number;
  alertClickThroughRate: number;
  sourceClickRate: number;
  explanationPolicyViolationRate: number;
  kpiHealth: string;
  recommendation: string;
  falseRelevanceRate: number;
  duplicateAlertRate: number;
  missingSourceRate: number;
  topFeedbackThemes: string[];
  ruleVersion: string;
  notes: string | null;
};

export type AiAuditSummary = {
  windowSize: number;
  totalLogs: number;
  fallbackUsedCount: number;
  fallbackRate: number;
  fallbackRateLevel: string;
  fallbackRateWarning: boolean;
  blockedTermDetections: number;
  topBlockedTerms: string[];
  topFailureCodes: string[];
};

export type LocalDataFileStatus = {
  name: string;
  sizeBytes: number;
  lastModifiedUtc: string;
};

export type LocalDataExport = {
  mode: string;
  localDataDirectory: string | null;
  files: LocalDataFileStatus[];
};

export type BetaStatus = {
  storage: StorageStatus;
  eventSummary: KpiEventsSummary;
  weeklyRollup: BetaWeeklyRollup;
  aiAuditSummary: AiAuditSummary;
  localData: LocalDataExport;
};
