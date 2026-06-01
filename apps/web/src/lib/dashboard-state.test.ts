import { describe, expect, it } from "vitest";
import { buildRefreshNotice, getFailuresCount, normalizeHoldingsErrorMessage } from "./dashboard-state";

describe("dashboard-state", () => {
  it("normalizes known holdings error codes", () => {
    expect(normalizeHoldingsErrorMessage("duplicate_symbol: Symbol already exists.")).toBe("This symbol is already in your holdings.");
    expect(normalizeHoldingsErrorMessage("invalid_symbol: bad format")).toBe("Symbol format is invalid.");
    expect(normalizeHoldingsErrorMessage("request_failed: 500")).toBe("Request failed. Please try again shortly.");
  });

  it("keeps unknown errors unchanged", () => {
    expect(normalizeHoldingsErrorMessage("some_unknown_error")).toBe("some_unknown_error");
  });

  it("builds refresh notice from failure count", () => {
    expect(buildRefreshNotice(0)).toBe("Refresh complete.");
    expect(buildRefreshNotice(3)).toBe("Refresh completed with 3 partial failure(s).");
  });

  it("counts rejected statuses", () => {
    expect(getFailuresCount(["fulfilled", "rejected", "fulfilled", "rejected"])).toBe(2);
  });
});
