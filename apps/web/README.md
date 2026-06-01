# MacroBrief Web App

## Purpose
Frontend dashboard app for holdings, macro brief, impact cards, alerts, and macro map.

## Stack
- Next.js (App Router)
- TypeScript

## Environment Variables
See `.env.example`.

## Run
```powershell
cd C:\Users\kbyun\Documents\MacroBrief\apps\web
npm install
npm run dev
```

## API Pairing
Run API in a separate terminal:

```powershell
cd C:\Users\kbyun\Documents\MacroBrief
$env:DOTNET_CLI_HOME='C:\Users\kbyun\Documents\MacroBrief\.dotnet'
dotnet run --project apps/api/src/MacroBrief.Api/MacroBrief.Api.csproj
```

Set `apps/web/.env.local`:

```env
NEXT_PUBLIC_API_BASE_URL=http://localhost:5221
NEXT_PUBLIC_APP_NAME=MacroBrief
NEXT_PUBLIC_ALERT_POLL_MINUTES=10
```

## Quick QA Checklist
- Dashboard first load renders summary/holdings/cards/alerts/map.
- Add holding works with Enter key and Add button.
- Duplicate symbol shows a friendly error message.
- Remove holding refreshes summary and related sections.
- Refresh button shows completion notice.
- Live Alerts shows `Updated at HH:MM:SS` after polling.
- Tab hidden -> return to tab -> Live Alerts refreshes immediately.

## Troubleshooting
- `Failed to fetch` or partial failures:
  - Confirm API is running and `NEXT_PUBLIC_API_BASE_URL` matches API port.
  - Restart `npm run dev` after changing `.env.local`.
- Hydration error with `data-wxt-integrated` or `__endic_crx__`:
  - Disable browser extension injecting page content (dictionary/translator/grammar tools).
  - Re-test in incognito/private window with extensions off.
