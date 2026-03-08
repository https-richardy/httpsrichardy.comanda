import { Input } from '@/shared/components/ui/input';
import type { ComponentProps } from 'react';

interface InputInstallmentProps extends ComponentProps<'input'> {
  onChangeHandler?: (value: string) => void;
}

export const InputInstallment = ({
  onChangeHandler,
  ...props
}: InputInstallmentProps) => {
  return (
    <div className="relative">
      <Input
        {...props}
        type="number"
        min="0"
        max="9999"
        placeholder="Ex: 6"
        onChange={(e) => onChangeHandler?.(e.target.value)}
      />
      <span className="absolute right-3 top-1/2 -translate-y-1/2 text-sm text-muted-foreground pointer-events-none">
        parcelas
      </span>
    </div>
  );
};
