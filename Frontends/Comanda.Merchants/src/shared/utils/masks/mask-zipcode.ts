export function maskZipCode(value?: string | null): string {
  if (!value) return '';
  const clean = value.replace(/\D/g, '').padStart(8, '0');

  if (clean.length === 8) {
    return clean.replace(/(\d{5})(\d{3})/, '$1-$2');
  }

  return value;
}
