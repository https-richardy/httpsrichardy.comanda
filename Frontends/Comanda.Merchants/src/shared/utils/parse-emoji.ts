import emojiData from 'emojibase-data/en/data.json';
import emojibaseShortcodes from 'emojibase-data/en/shortcodes/emojibase.json';
import githubShortcodes from 'emojibase-data/en/shortcodes/github.json';
import iamcalShortcodes from 'emojibase-data/en/shortcodes/iamcal.json';
import joypixelsShortcodes from 'emojibase-data/en/shortcodes/joypixels.json';

let shortcodeMap: Record<string, string> | null = null;

const getMap = () => {
  if (shortcodeMap) return shortcodeMap;

  shortcodeMap = {};

  const allShortcodeSources = [
    emojibaseShortcodes,
    githubShortcodes,
    iamcalShortcodes,
    joypixelsShortcodes
  ];

  // Mapeia hexcodes para emojis para busca rápida
  const hexToEmoji: Record<string, string> = {};
  for (const emoji of emojiData) {
    hexToEmoji[emoji.hexcode] = emoji.emoji;
  }

  // Processa cada fonte de shortcodes
  for (const source of allShortcodeSources) {
    for (const [hex, codes] of Object.entries(
      source as Record<string, string | string[]>
    )) {
      const emojiChar = hexToEmoji[hex];
      if (!emojiChar) continue;

      if (Array.isArray(codes)) {
        for (const code of codes) {
          shortcodeMap[code] = emojiChar;
        }
      } else {
        shortcodeMap[codes] = emojiChar;
      }
    }
  }

  return shortcodeMap;
};

/**
 * Converte shortcodes de emoji (ex: :smile:) em caracteres unicode reais.
 * Utiliza o emojibase-data com múltiplas fontes de shortcodes para cobertura máxima.
 */
export const parseEmoji = (text?: string | null): string => {
  if (!text) return '';

  const map = getMap();

  return text.replace(/:([a-zA-Z0-9_+-]+):/g, (match, key) => {
    return map[key] || match;
  });
};
