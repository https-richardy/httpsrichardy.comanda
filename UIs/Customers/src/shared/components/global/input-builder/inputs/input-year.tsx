import { InputWithSuffix } from '@/shared/components/ui/input-with-sufix';
import type { ComponentProps } from 'react';

interface InputYearProps extends ComponentProps<'input'> {
  onChangeHandler?: (value: string) => void;
}

export const InputYear = ({ onChangeHandler, ...props }: InputYearProps) => {
  return (
    <InputWithSuffix
      {...props}
      type="number"
      min="0"
      max="9999"
      placeholder="Ex: 6"
      onChange={(e) => onChangeHandler?.(e.target.value)}
      suffix="anos"
    />
  );
};
