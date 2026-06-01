"use client";

import { useEffect, useState } from "react";
import { apiClient } from "../lib/api-client";
import { formatTimeHms } from "../lib/dashboard-format";
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
    let inFlight = false;
    const tick = async () => {
      if (document.visibilityState !== "visible" || inFlight) return;
      inFlight = true;
      setPolling(true);
      try {
        const nextItems = await apiClient.getLiveAlerts(10);
        setItems(nextItems);
        setStatus(`Updated at ${formatTimeHms(new Date())}`);
      } catch {
        setStatus("Polling failed (showing last data)");
      } finally {
        setPolling(false);
        inFlight = false;
      }
    };

    const timer = setInterval(() => {
      void tick();
    }, pollMs);

    const onVisibilityChange = () => {
      if (document.visibilityState === "visible") {
        void tick();
      }
    };

    document.addEventListener("visibilitychange", onVisibilityChange);

    return () => {
      clearInterval(timer);
      document.removeEventListener("visibilitychange", onVisibilityChange);
    };
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
