import { Input } from '@/shared/components/ui/input';
import { maskInputMoneyBR } from '@/shared/utils/masks/mask-money';
import type { ComponentProps } from 'react';

interface InputMoneyProps extends ComponentProps<'input'> {
  onChangeHandler?: (value: string) => void;
}

const parseToNumber = (value: any) => {
  if (value === null || value === undefined || value === '') return undefined;
  if (typeof value === 'number') return value;

  let stringValue = String(value);

  if (stringValue.includes('.') && stringValue.includes(',')) {
    stringValue = stringValue.replace(/\./g, '').replace(',', '.');
  } else if (stringValue.includes(',')) {
    stringValue = stringValue.replace(',', '.');
  }

  const parsed = parseFloat(stringValue);
  return isNaN(parsed) ? undefined : parsed;
};

function formatCurrencyPTBR(value: any) {
  const number = parseToNumber(value);
  if (number === undefined || number === null || isNaN(number))
    return 'R$ 0,00';
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(number);
}

export const InputMoney = ({
  onChangeHandler,
  value,
  defaultValue,
  ...props
}: InputMoneyProps) => {
  const maskedValue = defaultValue
    ? formatCurrencyPTBR(defaultValue)
    : value
      ? formatCurrencyPTBR(value)
      : '';
  return (
    <Input
      {...props}
      value={maskedValue}
      onChange={(e) => {
        const masked = maskInputMoneyBR(e.target.value);
        onChangeHandler?.(masked);
      }}
    />
  );
};
