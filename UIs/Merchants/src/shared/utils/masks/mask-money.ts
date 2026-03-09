export const maskInputMoneyBR = (value: string): string => {
  const numbers = value.replace(/\D/g, '');
  const numberValue = Number(numbers) / 100;

  return numberValue.toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
};
