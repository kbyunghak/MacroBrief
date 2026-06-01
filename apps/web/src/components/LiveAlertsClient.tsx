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
  const [polling, setPolling] = useState<boolean>(false);

  useEffect(() => {
    const timer = setInterval(async () => {
      setPolling(true);
      try {
        const nextItems = await apiClient.getLiveAlerts(10);
        setItems(nextItems);
        setStatus("Updated");
      } catch {
        setStatus("Polling failed (showing last data)");
      } finally {
        setPolling(false);
      }
    }, pollMs);

    return () => clearInterval(timer);
  }, []);

  return (
    <>
      <p>
        <small>{polling ? "Refreshing..." : status}</small>
      </p>
      {items.length === 0 ? (
        <p>No live alerts yet.</p>
      ) : (
        <ul>
          {items.map((alert) => (
            <li key={alert.id}>
              {alert.category} / {alert.severity}: {alert.message}
            </li>
          ))}
        </ul>
      )}
    </>
  );
}
