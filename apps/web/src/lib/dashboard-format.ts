export function formatSummaryUpdatedLabel(isoDate: string): string {
  return new Date(isoDate).toISOString().replace("T", " ").replace("Z", " UTC");
}

export function formatTimeHms(date: Date): string {
  return date.toISOString().slice(11, 19);
}
