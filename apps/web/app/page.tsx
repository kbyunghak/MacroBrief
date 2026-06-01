import { apiClient } from "../src/lib/api-client";
import { ImpactCardsClient } from "../src/components/ImpactCardsClient";
import { LiveAlertsClient } from "../src/components/LiveAlertsClient";

export const dynamic = "force-dynamic";

export default async function Page() {
  try {
    const [summary, holdings, impactCards, liveAlerts, macroMap] = await Promise.all([
      apiClient.getDashboardSummary(),
      apiClient.getHoldings(),
      apiClient.getImpactCards(),
      apiClient.getLiveAlerts(10),
      apiClient.getMacroMap()
    ]);

    return (
      <main>
        <h1>MacroBrief Dashboard</h1>
        <p>Last updated: {new Date(summary.lastUpdatedAt).toLocaleString()}</p>

        <section>
          <h2>Summary</h2>
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
            {holdings.length === 0 ? <p>No holdings yet.</p> : <ul>{holdings.map((h) => <li key={h.symbol}>{h.symbol}</li>)}</ul>}
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

        <div className="grid two">
          <section>
            <h2>Impact Cards</h2>
            <ImpactCardsClient items={impactCards} />
          </section>

          <section>
            <h2>Live Alerts</h2>
            <LiveAlertsClient initialItems={liveAlerts} />
          </section>
        </div>

        <section>
          <h2>Macro Map</h2>
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
  } catch (error) {
    return (
      <main>
        <h1>MacroBrief Dashboard</h1>
        <section>
          <p className="error">Failed to load dashboard data.</p>
          <p>{error instanceof Error ? error.message : "Unknown error"}</p>
        </section>
      </main>
    );
  }
}
