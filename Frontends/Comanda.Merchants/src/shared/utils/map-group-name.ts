export function mapGroupName(group: string) {
  const normalized = group.replace(/^\//, '').toLowerCase();

  if (normalized === 'admins') return 'Administrador';
  if (normalized === 'operador' || normalized === 'operadores')
    return 'Operador';

  return group.replace(/^\//, '');
}
