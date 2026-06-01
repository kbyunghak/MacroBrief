"use client";

import { useState } from "react";
import { apiClient } from "../lib/api-client";
import type { ImpactCard } from "../types/api";

type Props = {
  items: ImpactCard[];
};

export function ImpactCardsClient({ items }: Props) {
  const [messageBySymbol, setMessageBySymbol] = useState<Record<string, string>>({});

  async function submit(symbol: string, feedback: "relevant" | "not_relevant") {
    try {
      await apiClient.submitRelevanceFeedback({
        newsEventId: `impact-${symbol.toLowerCase()}`,
        symbol,
        feedback
      });
      setMessageBySymbol((prev) => ({ ...prev, [symbol]: "Saved" }));
    } catch {
      setMessageBySymbol((prev) => ({ ...prev, [symbol]: "Failed" }));
    }
  }

  return (
    <ul>
      {items.map((card) => (
        <li key={`${card.symbol}-${card.headline}`} style={{ marginBottom: 8 }}>
          <strong>{card.symbol}</strong> [{card.impactLevel}] {card.headline}
          <div>
            <button onClick={() => submit(card.symbol, "relevant")}>Relevant</button>{" "}
            <button onClick={() => submit(card.symbol, "not_relevant")}>Not Relevant</button>{" "}
            <small>{messageBySymbol[card.symbol] ?? ""}</small>
          </div>
        </li>
      ))}
    </ul>
  );
}
