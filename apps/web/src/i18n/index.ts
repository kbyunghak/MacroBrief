import { enMessages } from "./en";
import { koMessages } from "./ko";

export const messages = {
  en: enMessages,
  ko: koMessages
} as const;

export type Locale = keyof typeof messages;

export const supportedLocales = Object.keys(messages) as Locale[];

export function resolveLocale(value?: string): Locale {
  return value === "ko" ? "ko" : "en";
}

export const defaultLocale: Locale = "en";

export function getMessages(locale: Locale = defaultLocale) {
  return messages[locale];
}
