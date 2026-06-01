"use client";

import { useState } from "react";
import { apiClient } from "../lib/api-client";
import { emitKpiEvent } from "../lib/kpi-events";
import type { ImpactCard } from "../types/api";

type Props = {
  items: ImpactCard[];
};

export function ImpactCardsClient({ items }: Props) {
  const [messageBySymbol, setMessageBySymbol] = useState<Record<string, string>>({});
  const [busyBySymbol, setBusyBySymbol] = useState<Record<string, boolean>>({});

  async function submit(symbol: string, feedback: "relevant" | "not_relevant") {
    setBusyBySymbol((prev) => ({ ...prev, [symbol]: true }));
    try {
      await apiClient.submitRelevanceFeedback({
        newsEventId: `impact-${symbol.toLowerCase()}`,
        symbol,
        feedback
      });
      void emitKpiEvent("impact_feedback", {
        symbol,
        newsEventId: `impact-${symbol.toLowerCase()}`,
        feedback
      });
      setMessageBySymbol((prev) => ({ ...prev, [symbol]: "Saved" }));
    } catch {
      setMessageBySymbol((prev) => ({ ...prev, [symbol]: "Failed" }));
    } finally {
      setBusyBySymbol((prev) => ({ ...prev, [symbol]: false }));
    }
  }

  if (items.length === 0) {
    return <p>No impact cards available.</p>;
  }

  return (
    <ul>
      {items.map((card) => (
        <li key={`${card.symbol}-${card.headline}`} style={{ marginBottom: 8 }}>
          <strong>{card.symbol}</strong> [{card.impactLevel}] {card.headline}
          <div>
            <button onClick={() => void submit(card.symbol, "relevant")} disabled={busyBySymbol[card.symbol]}>
              Relevant
            </button>{" "}
            <button onClick={() => void submit(card.symbol, "not_relevant")} disabled={busyBySymbol[card.symbol]}>
              Not Relevant
            </button>{" "}
            <small>{busyBySymbol[card.symbol] ? "Saving..." : (messageBySymbol[card.symbol] ?? "")}</small>
          </div>
        </li>
      ))}
    </ul>
  );
}
