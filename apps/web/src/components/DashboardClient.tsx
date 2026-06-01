"use client";

import { useMemo, useState } from "react";
import { apiClient } from "../lib/api-client";
import { ImpactCardsClient } from "./ImpactCardsClient";
import { LiveAlertsClient } from "./LiveAlertsClient";
import type { DashboardSummary, Holding, ImpactCard, LiveAlert, MacroMapItem } from "../types/api";

type Props = {
  initialSummary: DashboardSummary;
  initialHoldings: Holding[];
  initialImpactCards: ImpactCard[];
  initialLiveAlerts: LiveAlert[];
  initialMacroMap: MacroMapItem[];
};

export function DashboardClient({
  initialSummary,
  initialHoldings,
  initialImpactCards,
  initialLiveAlerts,
  initialMacroMap
}: Props) {
  const [summary, setSummary] = useState(initialSummary);
  const [holdings, setHoldings] = useState(initialHoldings);
  const [impactCards, setImpactCards] = useState(initialImpactCards);
  const [liveAlerts, setLiveAlerts] = useState(initialLiveAlerts);
  const [macroMap, setMacroMap] = useState(initialMacroMap);

  const [newSymbol, setNewSymbol] = useState("");
  const [holdingsBusy, setHoldingsBusy] = useState(false);
  const [insightsBusy, setInsightsBusy] = useState(false);
  const [holdingsError, setHoldingsError] = useState("");
  const [summaryError, setSummaryError] = useState("");
  const [insightsError, setInsightsError] = useState("");

  const pollingKey = useMemo(() => holdings.map((h) => h.symbol).join(","), [holdings]);
  const normalizedSymbol = newSymbol.trim().toUpperCase();
  const canAddHolding = !holdingsBusy && normalizedSymbol.length > 0;
  const lastUpdatedLabel = new Date(summary.lastUpdatedAt).toISOString().replace("T", " ").replace("Z", " UTC");

  function normalizeHoldingsErrorMessage(raw: string) {
    if (raw.includes("duplicate_symbol")) return "This symbol is already in your holdings.";
    if (raw.includes("invalid_symbol")) return "Symbol format is invalid.";
    if (raw.includes("request_failed")) return "Request failed. Please try again shortly.";
    return raw;
  }

  async function refreshAll() {
    const [nextSummary, nextHoldings, nextImpactCards, nextLiveAlerts, nextMacroMap] = await Promise.allSettled([
      apiClient.getDashboardSummary(),
      apiClient.getHoldings(),
      apiClient.getImpactCards(),
      apiClient.getLiveAlerts(10),
      apiClient.getMacroMap()
    ]);

    if (nextSummary.status === "fulfilled") {
      setSummary(nextSummary.value);
      setSummaryError("");
    } else {
      setSummaryError("Summary refresh failed. Showing last data.");
    }

    if (nextHoldings.status === "fulfilled") {
      setHoldings(nextHoldings.value);
      setHoldingsError("");
    } else {
      setHoldingsError("Holdings refresh failed. Showing last data.");
    }

    const insightsFailures: string[] = [];

    if (nextImpactCards.status === "fulfilled") {
      setImpactCards(nextImpactCards.value);
    } else {
      insightsFailures.push("impact cards");
    }

    if (nextLiveAlerts.status === "fulfilled") {
      setLiveAlerts(nextLiveAlerts.value);
    } else {
      insightsFailures.push("live alerts");
    }

    if (nextMacroMap.status === "fulfilled") {
      setMacroMap(nextMacroMap.value);
    } else {
      insightsFailures.push("macro map");
    }

    if (insightsFailures.length > 0) {
      setInsightsError(`Failed to refresh ${insightsFailures.join(", ")}. Showing last data.`);
    } else {
      setInsightsError("");
    }
  }

  async function onAddHolding() {
    const symbol = normalizedSymbol;
    if (!symbol) return;

    setHoldingsError("");
    setHoldingsBusy(true);
    setInsightsBusy(true);
    try {
      await apiClient.addHolding(symbol);
      setNewSymbol("");
      await refreshAll();
    } catch (error) {
      const message = error instanceof Error ? error.message : "Failed to add holding.";
      setHoldingsError(normalizeHoldingsErrorMessage(message));
    } finally {
      setHoldingsBusy(false);
      setInsightsBusy(false);
    }
  }

  async function onDeleteHolding(symbol: string) {
    setHoldingsError("");
    setHoldingsBusy(true);
    setInsightsBusy(true);
    try {
      await apiClient.deleteHolding(symbol);
      await refreshAll();
    } catch (error) {
      const message = error instanceof Error ? error.message : "Failed to remove holding.";
      setHoldingsError(normalizeHoldingsErrorMessage(message));
    } finally {
      setHoldingsBusy(false);
      setInsightsBusy(false);
    }
  }

  return (
    <main>
      <h1>MacroBrief Dashboard</h1>
      <p>Last updated: {lastUpdatedLabel}</p>

      <section>
        <h2>Summary</h2>
        {summaryError ? <p className="error">{summaryError}</p> : null}
        <p>Holdings: {summary.holdingsCount}</p>
        <p>Related updates: {summary.relatedUpdatesCount}</p>
        <ul>
          {summary.topExposureCategories.map((item) => (
            <li key={item}>{item}</li>
          ))}
        </ul>
      </section>

      <div className="grid two">
        <section>
          <h2>Holdings</h2>
          <div>
            <input
              value={newSymbol}
              onChange={(e) => {
                setNewSymbol(e.target.value.toUpperCase());
                if (holdingsError) setHoldingsError("");
              }}
              onKeyDown={(e) => {
                if (e.key === "Enter" && canAddHolding) {
                  e.preventDefault();
                  void onAddHolding();
                }
              }}
              placeholder="Add symbol (e.g. SOFI)"
              disabled={holdingsBusy}
            />{" "}
            <button onClick={() => void onAddHolding()} disabled={!canAddHolding}>
              Add
            </button>
          </div>
          {holdingsBusy ? <p>Updating holdings...</p> : null}
          {holdingsError ? <p className="error">{holdingsError}</p> : null}
          {holdings.length === 0 ? (
            <p>No holdings yet.</p>
          ) : (
            <ul>
              {holdings.map((h) => (
                <li key={h.symbol}>
                  {h.symbol}{" "}
                  <button onClick={() => onDeleteHolding(h.symbol)} disabled={holdingsBusy}>
                    Remove
                  </button>
                </li>
              ))}
            </ul>
          )}
        </section>

        <section>
          <h2>Morning Brief</h2>
          <ul>
            {summary.morningBrief.map((line) => (
              <li key={line}>{line}</li>
            ))}
          </ul>
        </section>
      </div>

      {insightsError ? <p className="error">{insightsError}</p> : null}

      <div className="grid two">
        <section>
          <h2>Impact Cards</h2>
          {insightsBusy ? <p>Refreshing impact data...</p> : null}
          <ImpactCardsClient items={impactCards} />
        </section>

        <section>
          <h2>Live Alerts</h2>
          {insightsBusy ? <p>Refreshing alerts...</p> : null}
          <LiveAlertsClient key={pollingKey} initialItems={liveAlerts} />
        </section>
      </div>

      <section>
        <h2>Macro Map</h2>
        {insightsBusy ? <p>Refreshing macro map...</p> : null}
        <ul>
          {macroMap.map((item) => (
            <li key={item.category}>
              {item.category}: {item.relatedHoldingsCount} holdings ({item.trendDirection})
            </li>
          ))}
        </ul>
      </section>
    </main>
  );
}
