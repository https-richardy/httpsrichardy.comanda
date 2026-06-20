export function getCustomerAddress(
  address: string | null | undefined,
  city: string | null | undefined,
  uf: string | null | undefined
): string {
  const cityState = [city, uf].filter(Boolean).join(' - ');
  const fullAddress = [address, cityState].filter(Boolean).join(', ');

  return fullAddress || 'Não informado';
}
