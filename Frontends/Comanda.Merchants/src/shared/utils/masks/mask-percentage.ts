export const maskPercent = (value: string, decimals: number = 2): string => {
  if (!value && value !== '0') return '';

  const numbers = value.replace(/\D/g, '');

  const numberValue = Number(numbers) / 10 ** decimals;

  return numberValue.toLocaleString('pt-BR', {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  });
};

export const unmaskPercent = (
  value: string | number,
  decimals: number = 2
): number | undefined => {
  if (typeof value === 'number') return value;
  if (!value) return undefined;
  const numbers = value.replace(/\D/g, '');
  if (!numbers) return undefined;
  return Number(numbers) / 10 ** decimals;
};
