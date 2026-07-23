"use client";

import { useState } from "react";
import { getMessages, type Locale } from "../i18n";

function TickerChip({
  ticker,
  label,
  tone = "blue"
}: {
  ticker: string;
  label: string;
  tone?: string;
}) {
  return (
    <button className={`ticker-chip ${tone}`} type="button" aria-label={`${ticker} ${label}`}>
      {ticker}
    </button>
  );
}

export function MacroFirstDashboard() {
  const [locale, setLocale] = useState<Locale>("en");
  const copy = getMessages(locale);

  return (
    <main className="dashboard-shell">
      <header className="top-nav">
        <div className="brand-lockup">
          <div className="brand-mark">MB</div>
          <div>
            <h1>{copy.app.name}</h1>
            <p>{copy.app.tagline}</p>
          </div>
        </div>
        <nav aria-label={copy.nav.ariaLabel}>
          <a className="active" href="#today">
            {copy.nav.today}
          </a>
          <a href="#alerts">{copy.nav.alerts}</a>
          <a href="#holdings">{copy.nav.holdings}</a>
          <a href="#recap">{copy.nav.recap}</a>
        </nav>
        <div className="top-nav-actions">
          <div className="locale-switch" aria-label="Language selector">
            <button
              className={locale === "en" ? "active" : ""}
              type="button"
              aria-pressed={locale === "en"}
              onClick={() => setLocale("en")}
            >
              EN
            </button>
            <button
              className={locale === "ko" ? "active" : ""}
              type="button"
              aria-pressed={locale === "ko"}
              onClick={() => setLocale("ko")}
            >
              KO
            </button>
            <span className={`locale-switch-thumb ${locale}`} aria-hidden="true" />
          </div>
          <p className="advice-note">{copy.app.adviceNote}</p>
        </div>
      </header>

      <section className="demo-status-strip" aria-label="Demo status">
        <span>{copy.demoStatus.mockData}</span>
        <span>{copy.demoStatus.staticDemo}</span>
        <span>
          {copy.demoStatus.updatedLabel}: {copy.demoStatus.updatedAtUtc}
        </span>
      </section>

      <section className="signal-strip" aria-label={copy.signals.ariaLabel}>
        {copy.signals.items.map((signal) => (
          <article className="signal-card" key={signal.label}>
            <p>{signal.label}</p>
            <strong className={signal.direction}>{signal.value}</strong>
            <span>{signal.detail}</span>
          </article>
        ))}
      </section>

      <div className="dashboard-grid">
        <section className="macro-brief" id="today">
          <div className="section-heading">
            <h2>{copy.macroBrief.title}</h2>
            <span>{copy.macroBrief.subtitle}</span>
          </div>
          <div className="macro-card-list">
            {copy.macroBrief.cards.map((card) => (
              <article className="macro-card" key={card.rank}>
                <div className={`rank-badge ${card.tone}`}>{card.rank}</div>
                <div className="macro-card-body">
                  <h3>{card.title}</h3>
                  <p>{card.summary}</p>
                  <div className="why-row">
                    <strong>{copy.macroBrief.whyItMatters}</strong>
                    <span>{card.why}</span>
                  </div>
                </div>
                <div className="related-panel">
                  <span>{copy.macroBrief.relatedHoldings}</span>
                  <div className="ticker-row">
                    {card.tickers.map((ticker) => (
                      <TickerChip
                        key={ticker}
                        ticker={ticker}
                        tone={card.tone}
                        label={copy.tickerChip.relatedHoldingLabel}
                      />
                    ))}
                  </div>
                  <a href={card.source} target="_blank" rel="noreferrer">
                    {copy.macroBrief.viewSource}
                  </a>
                </div>
              </article>
            ))}
          </div>
        </section>

        <aside className="side-stack">
          <section className="holdings-panel" id="holdings">
            <div className="section-heading compact">
              <h2>{copy.holdings.title}</h2>
              <button type="button">{copy.holdings.viewAll}</button>
            </div>
            <table>
              <thead>
                <tr>
                  <th>{copy.holdings.table.ticker}</th>
                  <th>{copy.holdings.table.preMarket}</th>
                  <th>{copy.holdings.table.mainMacro}</th>
                </tr>
              </thead>
              <tbody>
                {copy.holdings.items.map((holding) => (
                  <tr key={holding.ticker}>
                    <td>{holding.ticker}</td>
                    <td className={holding.move}>{holding.preMarket}</td>
                    <td>{holding.related}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </section>

          <section className="alerts-panel" id="alerts">
            <div className="section-heading compact">
              <h2>{copy.alerts.title}</h2>
              <span>{copy.alerts.delayLabel}</span>
            </div>
            <div className="alert-list">
              {copy.alerts.items.map((alert) => (
                <article className="alert-row" key={`${alert.time}-${alert.title}`}>
                  <span className={`alert-dot ${alert.severity}`} />
                  <time>{alert.time}</time>
                  <div>
                    <h3>{alert.title}</h3>
                    <div className="ticker-row">
                      {alert.tickers.map((ticker) => (
                        <TickerChip key={ticker} ticker={ticker} label={copy.tickerChip.relatedHoldingLabel} />
                      ))}
                    </div>
                  </div>
                </article>
              ))}
            </div>
          </section>
        </aside>
      </div>

      <section className="macro-map" id="recap">
        <div className="section-heading">
          <h2>{copy.macroMap.title}</h2>
          <span>{copy.macroMap.subtitle}</span>
        </div>
        <div className="macro-map-track">
          {copy.macroMap.items.map((item) => (
            <article className={`map-node ${item.tone}`} key={item.label}>
              <strong>{item.label}</strong>
              <div className="ticker-row">
                {item.tickers.map((ticker) => (
                  <TickerChip
                    key={ticker}
                    ticker={ticker}
                    tone={item.tone}
                    label={copy.tickerChip.relatedHoldingLabel}
                  />
                ))}
              </div>
            </article>
          ))}
        </div>
      </section>
    </main>
  );
}
