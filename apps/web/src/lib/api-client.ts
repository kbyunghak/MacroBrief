import type {
  ApiResponse,
  DashboardSummary,
  Holding,
  ImpactCard,
  BetaStatus,
  KpiEventRequest,
  LiveAlert,
  MacroMapItem,
  RelevanceFeedbackRequest
} from "../types/api";

const API_BASE_URL = (process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5221").replace(/\/+$/, "");

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers: {
      "Content-Type": "application/json",
      ...(init?.headers ?? {})
    }
  });

  const body = await response.text();
  const payload = body ? (JSON.parse(body) as ApiResponse<T>) : null;
  if (!response.ok || payload?.error) {
    const code = payload?.error?.code ?? "request_failed";
    const message = payload?.error?.message ?? `Request failed with status ${response.status}`;
    throw new Error(`${code}: ${message}`);
  }

  if (payload === null || payload.data === null) {
    throw new Error("invalid_response: response data was null");
  }

  return payload.data;
}

export const apiClient = {
  getDashboardSummary() {
    return request<DashboardSummary>("/api/v1/dashboard/summary");
  },
  getHoldings() {
    return request<Holding[]>("/api/v1/holdings");
  },
  addHolding(symbol: string) {
    return request<Holding>("/api/v1/holdings", {
      method: "POST",
      body: JSON.stringify({ symbol })
    });
  },
  deleteHolding(symbol: string) {
    return fetch(`${API_BASE_URL}/api/v1/holdings/${encodeURIComponent(symbol)}`, {
      method: "DELETE"
    }).then((response) => {
      if (!response.ok) {
        throw new Error(`request_failed: DELETE /holdings returned ${response.status}`);
      }
    });
  },
  getImpactCards(symbols?: string[]) {
    const query = symbols && symbols.length > 0 ? `?symbols=${encodeURIComponent(symbols.join(","))}` : "";
    return request<ImpactCard[]>(`/api/v1/impact-cards${query}`);
  },
  getLiveAlerts(limit = 20) {
    return request<LiveAlert[]>(`/api/v1/live-alerts?limit=${limit}`);
  },
  getMacroMap() {
    return request<MacroMapItem[]>("/api/v1/macro-map");
  },
  submitRelevanceFeedback(input: RelevanceFeedbackRequest) {
    return request<{ accepted: boolean }>("/api/v1/feedback/relevance", {
      method: "POST",
      body: JSON.stringify(input)
    });
  },
  submitKpiEvent(input: KpiEventRequest) {
    return request<{ accepted: boolean }>("/api/v1/events", {
      method: "POST",
      body: JSON.stringify(input)
    });
  },
  getBetaStatus() {
    return request<BetaStatus>("/api/v1/internal/beta/status");
  }
};
