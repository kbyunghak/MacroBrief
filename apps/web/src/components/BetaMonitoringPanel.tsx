import type { BetaStatus } from "../types/api";

type Props = {
  status: BetaStatus;
};

function formatPercent(value: number) {
  return `${Math.round(value * 100)}%`;
}

export function BetaMonitoringPanel({ status }: Props) {
  const eventCounts = Object.entries(status.eventSummary.eventTypeCounts);

  return (
    <section>
      <h2>Beta Monitoring</h2>
      <div className="grid two">
        <div>
          <h3>Storage</h3>
          <p>Mode: {status.storage.mode}</p>
          <p>Local files: {status.localData.files.length}</p>
        </div>
        <div>
          <h3>KPI Health</h3>
          <p>Health: {status.weeklyRollup.kpiHealth}</p>
          <p>Recommendation: {status.weeklyRollup.recommendation}</p>
        </div>
        <div>
          <h3>Events</h3>
          <p>Total events: {status.eventSummary.totalEvents}</p>
          {eventCounts.length === 0 ? (
            <p>No events yet.</p>
          ) : (
            <ul>
              {eventCounts.map(([type, count]) => (
                <li key={type}>
                  {type}: {count}
                </li>
              ))}
            </ul>
          )}
        </div>
        <div>
          <h3>Feedback</h3>
          <p>Sample size: {status.weeklyRollup.feedbackSampleSize}</p>
          <p>Positive ratio: {formatPercent(status.weeklyRollup.relevancePositiveRatio)}</p>
          <p>False relevance: {formatPercent(status.weeklyRollup.falseRelevanceRate)}</p>
        </div>
        <div>
          <h3>AI Guardrails</h3>
          <p>Total logs: {status.aiAuditSummary.totalLogs}</p>
          <p>Fallback rate: {formatPercent(status.aiAuditSummary.fallbackRate)}</p>
          <p>Fallback level: {status.aiAuditSummary.fallbackRateLevel}</p>
        </div>
        <div>
          <h3>Engagement</h3>
          <p>Alert CTR: {formatPercent(status.weeklyRollup.alertClickThroughRate)}</p>
          <p>Source click rate: {formatPercent(status.weeklyRollup.sourceClickRate)}</p>
          <p>Weekly active users: {status.weeklyRollup.weeklyActiveUsers}</p>
        </div>
      </div>
    </section>
  );
}
