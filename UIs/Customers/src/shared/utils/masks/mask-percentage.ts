export const maskPercent = (value: string, decimals: number = 2): string => {
  if (!value && value !== '0') return '';

  const numbers = value.replace(/\D/g, '');

  const numberValue = Number(numbers) / 10 ** decimals;

  return numberValue.toLocaleString('pt-BR', {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  });
};
