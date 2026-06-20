export function maskRg(value?: string | null): string {
  if (!value) return '';
  const clean = value.replace(/\D/g, '');

  if (clean.length === 11) {
    return clean.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }

  if (clean.length === 9) {
    return clean.replace(/(\d{2})(\d{3})(\d{3})(\d{1})/, '$1.$2.$3-$4');
  }

  if (clean.length === 8) {
    return clean.replace(/(\d{1})(\d{3})(\d{3})(\d{1})/, '$1.$2.$3-$4');
  }

  return value;
}
