export function normalizeHoldingsErrorMessage(raw: string): string {
  if (raw.includes("duplicate_symbol")) return "This symbol is already in your holdings.";
  if (raw.includes("invalid_symbol")) return "Symbol format is invalid.";
  if (raw.includes("request_failed")) return "Request failed. Please try again shortly.";
  return raw;
}

export function buildRefreshNotice(failuresCount: number): string {
  if (failuresCount <= 0) return "Refresh complete.";
  return `Refresh completed with ${failuresCount} partial failure(s).`;
}

export function getFailuresCount(statuses: Array<"fulfilled" | "rejected">): number {
  return statuses.filter((s) => s !== "fulfilled").length;
}
