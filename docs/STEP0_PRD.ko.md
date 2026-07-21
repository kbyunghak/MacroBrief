# Step 0 PRD v2 - MacroBrief

## 1. 제품 요약

MacroBrief는 사용자의 미국 주식 보유종목에 영향을 줄 수 있는 주요 매크로 흐름을 먼저 보여주는 대시보드입니다.

이 제품은 뉴스 리더가 아닙니다. 뉴스와 source link는 대시보드를 보조하는 자료이고, 핵심 경험은 포트폴리오와 관련 있는 매크로 브리프를 빠르게 이해하는 것입니다.

## 2. 문제

개인 투자자는 수많은 시장 뉴스를 쉽게 볼 수 있지만, 대부분의 도구는 매일 가장 필요한 질문에 직접 답하지 못합니다.

> 오늘 어떤 매크로 변화가 내 보유종목과 관련 있을까?

MacroBrief는 뉴스를 먼저 나열하지 않고, 하루의 주요 매크로 테마를 먼저 묶은 뒤 어떤 보유종목과 연결되는지 보여줍니다.

## 3. 대상 사용자

주요 사용자:
- 미국 주식 보유종목이나 관심종목을 관리하는 개인 투자자
- 성장주, AI, 에너지, 금융, 소비자 신용, 중국 노출 기업처럼 매크로 민감도가 있는 종목을 추적하는 사용자
- 장 시작 전이나 변동성이 큰 장중에 빠르게 맥락을 파악하고 싶은 사용자

초기 포트폴리오 예시:
- `TSLA`
- `NVDA`
- `SOFI`
- `XOM`
- `CVX`

## 4. 제품 포지셔닝

MacroBrief는 다음에 가깝습니다.
- 매크로 우선 포트폴리오 대시보드
- 투자 조언이 아닌 정보 제공 제품
- source link와 timestamp를 유지하는 요약 도구
- 규칙 기반 매핑을 우선하고 AI는 보조로 사용하는 제품
- MVP 단계에서는 로컬 우선으로 동작하는 제품

MacroBrief는 다음이 아닙니다.
- 일반 뉴스 앱
- 증권사 또는 브로커리지 제품
- 매매 신호 제품
- 매수/매도 추천 도구
- 가격 예측 또는 목표가 제공 제품

## 5. 제품 원칙

- 개별 뉴스보다 매크로 맥락을 먼저 보여준다.
- 매크로 테마와 보유종목의 연결은 ticker chip으로 명확히 표시한다.
- 더 자세히 보고 싶은 사용자를 위해 source link를 제공한다.
- 가격 움직임을 예측하지 않고 관련성을 설명한다.
- AI 요약보다 결정적 규칙 매핑을 먼저 사용한다.
- MVP 개발 중에는 개인 데이터를 로컬에 보관하는 방식을 우선한다.

## 6. MVP 사용자 흐름

1. 사용자가 MacroBrief를 연다.
2. 대시보드는 portfolio mood, futures, yields, oil, dollar 같은 시장 신호 카드를 보여준다.
3. Morning Macro Brief에서 가장 중요한 매크로 카드를 보여준다.
4. 각 매크로 카드는 다음을 포함한다.
   - 매크로 headline
   - 짧은 context
   - why it matters 설명
   - 관련 보유종목 ticker chip/button
   - 자세히 보기 위한 source link
5. Holdings table은 각 ticker와 주요 관련 매크로 테마를 보여준다.
6. Live alerts는 포트폴리오와 관련 있는 업데이트를 ticker chip과 함께 보여준다.
7. Macro map은 매크로 테마와 보유종목의 관계를 시각적으로 보여준다.

## 7. MVP 범위

포함:
- Macro-first mock dashboard prototype
- Market signal strip
- Morning Macro Brief cards
- 관련 보유종목 ticker chip/button
- 주요 매크로 노출을 보여주는 holdings table
- Live portfolio alerts
- Macro map
- Macro card의 source link
- 개인 로컬 데이터를 위한 local JSON persistence
- Beta monitoring endpoint와 dashboard panel
- KPI events, feedback, guardrail audit 기반 구조

추후 계획:
- 실제 news provider 연결
- 실제 market data provider 연결
- guardrail 뒤에 실제 AI explanation provider 연결
- 다중 사용자 배포를 위한 persistent database
- 인증과 사용자별 데이터 분리
- production deployment

제외:
- 매수/매도/보유 추천
- 가격 목표
- 주가 예측 점수
- 거래 실행
- 브로커리지 연동
- 유료 기능 gating
- 앱 내부 전체 뉴스 리더

## 8. 대시보드 정보 구조

첫 화면 우선순위:
1. App identity와 navigation
2. Information-only disclaimer
3. Market signal strip
4. Morning Macro Brief
5. My Holdings
6. Live Portfolio Alerts
7. Macro Map

Macro card model:
- `title`
- `summary`
- `whyItMatters`
- `relatedHoldings`
- `macroTheme`
- `sourceName`
- `sourceUrl`
- `timestamp`
- `severity`

Ticker chip 동작:
- chip은 매크로 테마와 관련 있는 보유종목을 표시한다.
- 이후 filter나 상세 이동 기능으로 확장할 수 있다.
- chip은 추천이나 방향성 판단처럼 보이면 안 된다.

Source link 동작:
- source link는 더 자세히 읽기 위한 경로다.
- MacroBrief는 전체 뉴스 기사를 재작성하지 않고 관련성만 요약한다.

## 9. 데이터 전략

현재 단계:
- Macro-first Web prototype은 mock data로 동작한다.
- API 기반 components는 추후 통합을 위해 유지한다.
- 개인 로컬 데이터는 local JSON 파일로 저장할 수 있다.
- `.env.local`과 `.local-data/`는 커밋하지 않는다.

다음 데이터 단계:
- news와 market data provider interface를 정의한다.
- provider key는 local 또는 deployment secret에만 둔다.
- 외부 event를 기존 macro theme contract로 정규화한다.
- 화면에 노출되는 모든 item은 source URL, source name, timestamp를 유지한다.

Database 판단:
- 단일 사용자 로컬 개발에는 cloud database가 필요하지 않다.
- 다중 사용자 auth, cross-device sync, hosted beta, event history, deduplication, monitoring이 필요해질 때 database가 유용해진다.

## 10. 안전 및 컴플라이언스 경계

MacroBrief는 정보 제공 제품으로 유지해야 한다.

허용되는 표현:
- "This may be relevant because..."
- "This can affect..."
- "This theme is related to..."
- "Source context suggests..."

금지되는 표현:
- "Buy"
- "Sell"
- "Hold"
- "This stock will rise"
- "This stock will fall"
- "Price target"
- "Guaranteed"
- 조언처럼 사용되는 "bullish/bearish call"

생성된 설명은 guardrail validation을 통과해야 한다. 실패하면 안전한 fallback explanation을 사용한다.

## 11. 성공 지표

Prototype validation:
- 사용자가 첫 화면에서 macro-first dashboard라는 점을 이해한다.
- 사용자가 어떤 매크로 테마가 어떤 보유종목과 연결되는지 파악할 수 있다.
- 사용자가 자세한 맥락을 위해 source link를 자연스럽게 사용한다.
- 사용자가 앱을 투자 조언으로 오해하지 않는다.

Beta metrics:
- Daily brief open rate
- Alert click-through rate
- Source click rate
- Positive relevance feedback ratio
- False relevance rate
- Weekly active users
- AI fallback rate

## 12. 로드맵 정렬

완료된 기반:
- Step 0 PRD baseline
- Step 1 dashboard wireframe/spec
- Step 2 mapping rules/spec
- Step 3 ingestion/API spec
- Step 4 dashboard integration
- Step 5 AI guardrails
- Step 6 beta validation
- Step 7 portfolio packaging
- Post-Step 7 local-first persistence

현재 제품 방향:
- Step 8: macro-first prototype and documentation alignment

추천 다음 단계:
1. 현재 macro-first prototype baseline을 commit/push한다.
2. 로컬 Web smoke test를 진행한다.
3. responsive dashboard layout을 다듬는다.
4. mock data를 별도 prototype data module로 분리한다.
5. 선택된 card를 feature boundary 뒤에서 API-backed data와 다시 연결한다.
6. prototype 흐름이 안정된 뒤 실제 provider integration을 추가한다.

## 13. 문서 언어 정책

프로젝트 문서는 영어를 기준 문서로 사용합니다.

한글 문서가 필요하면 같은 주제의 sibling file을 `.ko.md` suffix로 만듭니다. 예:
- `docs/STEP0_PRD.md`
- `docs/STEP0_PRD.ko.md`

영어 문서는 구현 기준으로 유지하고, 한글 문서는 같은 방향을 더 읽기 쉬운 설명으로 제공합니다.

문서와 언어 파일은 UTF-8로 저장합니다.

## 14. Web 언어팩 정책

제품 경험에 직접 보이는 Web UI 문구는 component에서 분리해 language pack으로 관리합니다.

현재 language pack 위치:
- `apps/web/src/i18n/en.ts`
- `apps/web/src/i18n/ko.ts`
- `apps/web/src/i18n/index.ts`

현재 지원 locale:
- `en`
- `ko`

기본 동작:
- 기본 locale은 `en`입니다.
- macro-first dashboard 상단의 language switch에서 `EN`과 `KO`를 전환할 수 있습니다.

커밋 checkpoint 기준:
- 제품 방향 문서와 첫 화면 language pack은 같은 checkpoint로 묶습니다.
- responsive visual polish는 별도 checkpoint로 분리합니다.
- 실제 provider/API integration은 prototype 흐름이 안정된 뒤 나중 checkpoint에서 진행합니다.

---

Version: v2
Date: 2026-07-20
Owner: Andrew
