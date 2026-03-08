import { InputWithSuffix } from '@/shared/components/ui/input-with-sufix';
import type { ComponentProps } from 'react';

interface InputMonthsProps extends ComponentProps<'input'> {
  onChangeHandler?: (value: string) => void;
}

export const InputMonth = ({ onChangeHandler, ...props }: InputMonthsProps) => {
  return (
    <InputWithSuffix
      {...props}
      type="number"
      min="0"
      max="12"
      placeholder="Ex: 6"
      onChange={(e) => onChangeHandler?.(e.target.value)}
      suffix="meses"
    />
  );
};
