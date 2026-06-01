import { apiClient } from "./api-client";
import type { KpiEventRequest, KpiEventType } from "../types/api";

const USER_ID_KEY = "macrobrief_user_id";
const SESSION_ID_KEY = "macrobrief_session_id";

function randomId(prefix: string): string {
  return `${prefix}-${Math.random().toString(36).slice(2, 10)}-${Date.now().toString(36)}`;
}

function getOrCreateStorageValue(key: string, prefix: string): string {
  if (typeof window === "undefined") return `${prefix}-server`;
  const existing = window.localStorage.getItem(key);
  if (existing) return existing;
  const next = randomId(prefix);
  window.localStorage.setItem(key, next);
  return next;
}

function baseEvent(eventType: KpiEventType): Pick<KpiEventRequest, "eventId" | "eventType" | "userId" | "occurredAtUtc" | "sessionId"> {
  return {
    eventId: randomId("evt"),
    eventType,
    userId: getOrCreateStorageValue(USER_ID_KEY, "usr"),
    occurredAtUtc: new Date().toISOString(),
    sessionId: getOrCreateStorageValue(SESSION_ID_KEY, "ses")
  };
}

export async function emitKpiEvent(eventType: KpiEventType, extra?: Partial<KpiEventRequest>): Promise<void> {
  try {
    await apiClient.submitKpiEvent({
      ...baseEvent(eventType),
      ...(extra ?? {})
    });
  } catch {
    // Non-blocking telemetry path
  }
}
