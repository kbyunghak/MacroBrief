import { describe, expect, it } from "vitest";
import { formatSummaryUpdatedLabel, formatTimeHms } from "./dashboard-format";

describe("dashboard-format", () => {
  it("formats summary updated label in stable UTC string", () => {
    const value = formatSummaryUpdatedLabel("2026-06-01T16:17:11.036Z");
    expect(value).toBe("2026-06-01 16:17:11.036 UTC");
  });

  it("formats time as HH:MM:SS", () => {
    const value = formatTimeHms(new Date("2026-06-01T16:17:11.036Z"));
    expect(value).toBe("16:17:11");
  });
});
