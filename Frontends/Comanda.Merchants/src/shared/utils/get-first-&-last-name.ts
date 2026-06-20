export function getFirstNameAndLastName(fullName?: string) {
  if (!fullName) return { first: 'Lead', last: 'Detalhes' };
  const parts = fullName.trim().split(' ');
  return {
    first: parts[0],
    last: parts.length > 1 ? parts[parts.length - 1] : ''
  };
}
