export function formatGender(gender?: string) {
  if (!gender) return 'NÃO INFORMADO';
  const g = gender.toUpperCase();
  if (g.startsWith('M')) return 'MASC';
  if (g.startsWith('F')) return 'FEM';
  return g;
}
