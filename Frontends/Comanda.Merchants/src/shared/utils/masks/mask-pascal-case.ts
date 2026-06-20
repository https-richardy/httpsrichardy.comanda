export function maskPascalCase(value: string): string {
  return value
    .toLocaleLowerCase()
    .replace(/\b\w/g, (char) => char.toUpperCase());
}
