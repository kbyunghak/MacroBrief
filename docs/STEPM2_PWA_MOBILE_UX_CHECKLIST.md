# Step M2 PWA Mobile UX Checklist v1

## Goal
Ensure the PWA experience is genuinely usable on phones before native app work.

## Layout
- Mobile-first breakpoints
- Sticky top summary bar
- Collapsible sections for cards and alerts
- Large tap targets (min 44px)

## Performance
- First meaningful render under 3s on mid devices
- Lazy-load heavy sections
- Cache last dashboard payload for quick reopen

## Usability
- Holdings add/remove in <= 2 taps after opening panel
- Alert detail expandable without route jump
- Clear skeleton states for all sections

## PWA Basics
- Valid manifest with app name/icons/theme
- Service worker cache strategy for shell + recent data
- Install prompt support
- Offline fallback screen

## Accessibility
- Contrast AA minimum
- Screen reader labels for key actions
- Keyboard focus visibility (for tablet/desktop continuity)

## Exit Criteria
- Mobile layout checklist complete
- Performance and installability confirmed

---
Version: v1
Date: 2026-05-31
Owner: Andrew
