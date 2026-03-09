import type { ComponentProps } from 'react';
import { Input } from '@/shared/components/ui/input';
import { InputPercent } from './inputs/input-percent';
import { InputMonth } from './inputs/input-month';
import { InputYear } from './inputs/input-year';
import { InputInstallment } from './inputs/input-installment';
import { InputMoney } from './inputs/input-money';

export enum InputType {
  Money = 'Money',
  Percent = 'Percent',
  Month = 'Month',
  Year = 'Year',
  Installments = 'Installments'
}

interface InputBuilderProps extends ComponentProps<'input'> {
  onChangeHandler?: (value: string) => void;
  inputType: InputType;
}


const INPUT_STRATEGIES: Record<InputType, React.ElementType> = {
  [InputType.Money]: InputMoney,
  [InputType.Percent]: InputPercent,
  [InputType.Month]: InputMonth,
  [InputType.Year]: InputYear,
  [InputType.Installments]: InputInstallment
};

export const InputBuilder = ({
  inputType,
  onChangeHandler,
  ...props
}: InputBuilderProps) => {
  const InputComponent = INPUT_STRATEGIES[inputType];

  if (InputComponent) {
    return <InputComponent {...props} onChangeHandler={onChangeHandler} />;
  }

  return (
    <Input {...props} onChange={(e) => onChangeHandler?.(e.target.value)} />
  );
};
