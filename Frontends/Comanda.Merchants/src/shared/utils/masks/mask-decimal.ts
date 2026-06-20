export const maskDecimal = (value: string) => {
  let newValue = value.replace(/\D/g, '');
  newValue = newValue.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');

  if (newValue === '') newValue = '0';

  newValue = newValue.replace(/^0+(\d)/, '$1');

  if (newValue === '') newValue = '0';

  return newValue;
};

export const formatCurrencyBRL = (value: number | undefined | null): string => {
  if (typeof value !== 'number') return 'R$ 0,00';

  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
};
