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
