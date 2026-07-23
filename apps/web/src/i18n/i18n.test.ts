import { describe, expect, it } from "vitest";
import { defaultLocale, getMessages, messages, resolveLocale, supportedLocales } from ".";

describe("i18n", () => {
  it("defaults to English", () => {
    expect(defaultLocale).toBe("en");
    expect(getMessages().app.name).toBe("MacroBrief");
  });

  it("resolves unsupported locale values to English", () => {
    expect(resolveLocale("ko")).toBe("ko");
    expect(resolveLocale("en")).toBe("en");
    expect(resolveLocale("fr")).toBe("en");
    expect(resolveLocale(undefined)).toBe("en");
  });

  it("keeps English and Korean language packs structurally aligned", () => {
    expect(supportedLocales).toEqual(["en", "ko"]);
    expect(Object.keys(messages.ko)).toEqual(Object.keys(messages.en));
    expect(Object.keys(messages.ko.demoStatus)).toEqual(Object.keys(messages.en.demoStatus));
    expect(messages.ko.macroBrief.cards).toHaveLength(messages.en.macroBrief.cards.length);
    expect(messages.ko.holdings.items).toHaveLength(messages.en.holdings.items.length);
    expect(messages.ko.alerts.items).toHaveLength(messages.en.alerts.items.length);
    expect(messages.ko.macroMap.items).toHaveLength(messages.en.macroMap.items.length);
  });

  it("labels the public dashboard as a mock static demo", () => {
    expect(messages.en.demoStatus.mockData).toBe("MOCK DATA");
    expect(messages.en.demoStatus.staticDemo).toBe("STATIC DEMO");
    expect(messages.en.demoStatus.updatedAtUtc).toMatch(/UTC$/);
  });
});
