export const maskInputMoneyBR = (value: string): string => {
  const numbers = value.replace(/\D/g, '');
  const numberValue = Number(numbers) / 100;

  return numberValue.toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
};

export const unmaskMoney = (value: string | number): number | undefined => {
  if (typeof value === 'number') return value;
  if (!value) return undefined;
  const numbers = value.replace(/\D/g, '');
  if (!numbers) return undefined;
  return Number(numbers) / 100;
};
