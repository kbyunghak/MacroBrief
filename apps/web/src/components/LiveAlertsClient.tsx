"use client";

import { useEffect, useState } from "react";
import { apiClient } from "../lib/api-client";
import type { LiveAlert } from "../types/api";

type Props = {
  initialItems: LiveAlert[];
};

const pollMinutes = Number(process.env.NEXT_PUBLIC_ALERT_POLL_MINUTES ?? "10");
const pollMs = Math.max(1, pollMinutes) * 60 * 1000;

export function LiveAlertsClient({ initialItems }: Props) {
  const [items, setItems] = useState<LiveAlert[]>(initialItems);
  const [status, setStatus] = useState<string>("Live");

  useEffect(() => {
    const timer = setInterval(async () => {
      try {
        const nextItems = await apiClient.getLiveAlerts(10);
        setItems(nextItems);
        setStatus(`Updated ${new Date().toLocaleTimeString()}`);
      } catch {
        setStatus("Polling failed (showing last data)");
      }
    }, pollMs);

    return () => clearInterval(timer);
  }, []);

  return (
    <>
      <p>
        <small>{status}</small>
      </p>
      <ul>
        {items.map((alert) => (
          <li key={alert.id}>
            {alert.category} / {alert.severity}: {alert.message}
          </li>
        ))}
      </ul>
    </>
  );
}
