import { apiClient } from "../src/lib/api-client";
import { DashboardClient } from "../src/components/DashboardClient";

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
      <DashboardClient
        initialSummary={summary}
        initialHoldings={holdings}
        initialImpactCards={impactCards}
        initialLiveAlerts={liveAlerts}
        initialMacroMap={macroMap}
      />
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
