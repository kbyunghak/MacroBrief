import type { DashboardMessages } from "./en";

export const koMessages = {
  app: {
    name: "MacroBrief",
    tagline: "오늘 내 포트폴리오에 영향을 줄 수 있는 매크로 흐름",
    adviceNote: "정보 제공용 - 투자 조언이 아닙니다"
  },
  nav: {
    ariaLabel: "주요 메뉴",
    today: "오늘",
    alerts: "알림",
    holdings: "보유종목",
    recap: "요약"
  },
  demoStatus: {
    mockData: "모의 데이터",
    staticDemo: "정적 데모",
    updatedLabel: "업데이트",
    updatedAtUtc: "2026-07-23 00:00 UTC"
  },
  signals: {
    ariaLabel: "시장 신호",
    items: [
      { label: "포트폴리오 분위기", value: "혼조", direction: "neutral", detail: "보유종목별 매크로 압력이 엇갈림" },
      { label: "S&P 선물", value: "-0.4%", direction: "down", detail: "장전 흐름" },
      { label: "나스닥 선물", value: "-0.8%", direction: "down", detail: "성장주 부담" },
      { label: "10년물 금리", value: "4.42%", direction: "up", detail: "금리 상승" },
      { label: "유가 (WTI)", value: "+1.3%", direction: "up", detail: "공급 리스크" },
      { label: "DXY", value: "+0.2%", direction: "up", detail: "달러 강세" }
    ]
  },
  macroBrief: {
    title: "모닝 매크로 브리프",
    subtitle: "매크로 테마를 먼저, 보유종목은 그 다음에",
    whyItMatters: "왜 중요한가:",
    relatedHoldings: "관련 보유종목",
    viewSource: "출처 보기",
    cards: [
      {
        rank: "1",
        title: "개장 전 국채금리 상승",
        summary: "높아진 금리는 성장주 밸류에이션과 차입 민감 섹터에 부담이 될 수 있습니다.",
        why: "금리 상승은 밸류에이션, 자동차 금융, 대출 수요에 영향을 줄 수 있습니다.",
        tickers: ["TSLA", "NVDA", "SOFI"],
        tone: "blue",
        source: "https://www.federalreserve.gov/"
      },
      {
        rank: "2",
        title: "공급 리스크 우려로 유가 상승",
        summary: "지정학적 헤드라인이 원유 가격 기대를 높이며 에너지 시장이 강한 흐름을 보입니다.",
        why: "에너지 보유종목은 원유 가격 기대 변화에 반응할 수 있습니다.",
        tickers: ["XOM", "CVX"],
        tone: "green",
        source: "https://www.eia.gov/"
      },
      {
        rank: "3",
        title: "AI 인프라 투자 심리 견조",
        summary: "데이터센터와 AI 투자 기대가 반도체 섹터 심리를 계속 지지하고 있습니다.",
        why: "AI 투자 흐름은 반도체 주도주의 상대적 흐름에 영향을 줄 수 있습니다.",
        tickers: ["NVDA"],
        tone: "purple",
        source: "https://www.nasdaq.com/"
      }
    ]
  },
  holdings: {
    title: "내 보유종목",
    viewAll: "전체 보기",
    table: {
      ticker: "티커",
      preMarket: "장전 등락",
      mainMacro: "주요 관련 매크로"
    },
    items: [
      { ticker: "TSLA", preMarket: "-2.1%", related: "금리, 중국 EV, 신용", move: "negative" },
      { ticker: "NVDA", preMarket: "-1.4%", related: "AI, 금리, 수출 규제", move: "negative" },
      { ticker: "SOFI", preMarket: "-2.8%", related: "금리, 신용 리스크", move: "negative" },
      { ticker: "XOM", preMarket: "+1.2%", related: "유가, 지정학", move: "positive" },
      { ticker: "CVX", preMarket: "+0.9%", related: "유가, OPEC+, 달러", move: "positive" }
    ]
  },
  alerts: {
    title: "실시간 포트폴리오 알림",
    delayLabel: "약 15분 지연",
    items: [
      { time: "10:12 AM", title: "Fed 발언이 성장주 밸류에이션에 부담을 줄 수 있음", tickers: ["TSLA", "NVDA", "SOFI"], severity: "red" },
      { time: "11:03 AM", title: "지정학적 리스크로 유가 급등", tickers: ["XOM", "CVX"], severity: "orange" },
      { time: "11:26 AM", title: "AI 투자 전망 견조", tickers: ["NVDA"], severity: "purple" }
    ]
  },
  macroMap: {
    title: "매크로 맵",
    subtitle: "주요 매크로 테마와 보유종목의 연결",
    items: [
      { label: "금리", tickers: ["TSLA", "SOFI", "NVDA"], tone: "blue" },
      { label: "유가와 지정학", tickers: ["XOM", "CVX"], tone: "green" },
      { label: "AI / 반도체", tickers: ["NVDA"], tone: "purple" },
      { label: "소비자 신용", tickers: ["TSLA", "SOFI"], tone: "orange" }
    ]
  },
  tickerChip: {
    relatedHoldingLabel: "관련 보유종목"
  }
} satisfies DashboardMessages;
