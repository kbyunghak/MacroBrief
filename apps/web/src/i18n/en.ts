type SignalDirection = "neutral" | "up" | "down";
type MoveDirection = "positive" | "negative";

type Tone = "blue" | "green" | "purple" | "orange" | "red";

export type DashboardMessages = {
  app: {
    name: string;
    tagline: string;
    adviceNote: string;
  };
  nav: {
    ariaLabel: string;
    today: string;
    alerts: string;
    holdings: string;
    recap: string;
  };
  signals: {
    ariaLabel: string;
    items: Array<{
      label: string;
      value: string;
      direction: SignalDirection;
      detail: string;
    }>;
  };
  macroBrief: {
    title: string;
    subtitle: string;
    whyItMatters: string;
    relatedHoldings: string;
    viewSource: string;
    cards: Array<{
      rank: string;
      title: string;
      summary: string;
      why: string;
      tickers: string[];
      tone: Tone;
      source: string;
    }>;
  };
  holdings: {
    title: string;
    viewAll: string;
    table: {
      ticker: string;
      preMarket: string;
      mainMacro: string;
    };
    items: Array<{
      ticker: string;
      preMarket: string;
      related: string;
      move: MoveDirection;
    }>;
  };
  alerts: {
    title: string;
    delayLabel: string;
    items: Array<{
      time: string;
      title: string;
      tickers: string[];
      severity: Tone;
    }>;
  };
  macroMap: {
    title: string;
    subtitle: string;
    items: Array<{
      label: string;
      tickers: string[];
      tone: Tone;
    }>;
  };
  tickerChip: {
    relatedHoldingLabel: string;
  };
};

export const enMessages = {
  app: {
    name: "MacroBrief",
    tagline: "Today's macro impact on your portfolio",
    adviceNote: "Information only - not investment advice"
  },
  nav: {
    ariaLabel: "Primary",
    today: "Today",
    alerts: "Alerts",
    holdings: "Holdings",
    recap: "Recap"
  },
  signals: {
    ariaLabel: "Market signals",
    items: [
      { label: "Portfolio Mood", value: "Mixed", direction: "neutral", detail: "Macro pressure uneven across holdings" },
      { label: "S&P Futures", value: "-0.4%", direction: "down", detail: "Pre-market" },
      { label: "Nasdaq Futures", value: "-0.8%", direction: "down", detail: "Growth pressure" },
      { label: "10Y Yield", value: "4.42%", direction: "up", detail: "Rates higher" },
      { label: "Oil (WTI)", value: "+1.3%", direction: "up", detail: "Supply risk" },
      { label: "DXY", value: "+0.2%", direction: "up", detail: "Dollar firm" }
    ]
  },
  macroBrief: {
    title: "Morning Macro Brief",
    subtitle: "Macro themes first, holdings second",
    whyItMatters: "Why it matters:",
    relatedHoldings: "Related holdings",
    viewSource: "View source",
    cards: [
      {
        rank: "1",
        title: "Treasury yields move higher before the open",
        summary: "Higher yields may pressure growth-stock valuations and borrowing-sensitive sectors.",
        why: "Higher rates can affect valuations, auto financing, and loan demand.",
        tickers: ["TSLA", "NVDA", "SOFI"],
        tone: "blue",
        source: "https://www.federalreserve.gov/"
      },
      {
        rank: "2",
        title: "Crude oil rises on supply-risk concerns",
        summary: "Energy markets are firmer as geopolitical headlines lift crude price expectations.",
        why: "Energy holdings may react to changes in crude price expectations.",
        tickers: ["XOM", "CVX"],
        tone: "green",
        source: "https://www.eia.gov/"
      },
      {
        rank: "3",
        title: "AI infrastructure sentiment remains strong",
        summary: "Data center and AI spending expectations continue to support semiconductor sentiment.",
        why: "AI spending trends may influence semiconductor leadership.",
        tickers: ["NVDA"],
        tone: "purple",
        source: "https://www.nasdaq.com/"
      }
    ]
  },
  holdings: {
    title: "My Holdings",
    viewAll: "View all",
    table: {
      ticker: "Ticker",
      preMarket: "Pre-market",
      mainMacro: "Main related macro"
    },
    items: [
      { ticker: "TSLA", preMarket: "-2.1%", related: "Rates, China EV, Credit", move: "negative" },
      { ticker: "NVDA", preMarket: "-1.4%", related: "AI, Yields, Export Controls", move: "negative" },
      { ticker: "SOFI", preMarket: "-2.8%", related: "Rates, Credit Risk", move: "negative" },
      { ticker: "XOM", preMarket: "+1.2%", related: "Oil, Geopolitics", move: "positive" },
      { ticker: "CVX", preMarket: "+0.9%", related: "Oil, OPEC+, USD", move: "positive" }
    ]
  },
  alerts: {
    title: "Live Portfolio Alerts",
    delayLabel: "~15 min delayed",
    items: [
      { time: "10:12 AM", title: "Fed comments may pressure growth valuations", tickers: ["TSLA", "NVDA", "SOFI"], severity: "red" },
      { time: "11:03 AM", title: "Oil jumps on geopolitical risk", tickers: ["XOM", "CVX"], severity: "orange" },
      { time: "11:26 AM", title: "AI spending outlook remains firm", tickers: ["NVDA"], severity: "purple" }
    ]
  },
  macroMap: {
    title: "Macro Map",
    subtitle: "Key macro themes and their impact on your holdings",
    items: [
      { label: "Interest Rates", tickers: ["TSLA", "SOFI", "NVDA"], tone: "blue" },
      { label: "Oil & Geopolitics", tickers: ["XOM", "CVX"], tone: "green" },
      { label: "AI / Semiconductors", tickers: ["NVDA"], tone: "purple" },
      { label: "Consumer Credit", tickers: ["TSLA", "SOFI"], tone: "orange" }
    ]
  },
  tickerChip: {
    relatedHoldingLabel: "related holding"
  }
} satisfies DashboardMessages;
