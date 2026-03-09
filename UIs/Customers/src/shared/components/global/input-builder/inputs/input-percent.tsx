import { Input } from '@/shared/components/ui/input';
import { maskPercent } from '@/shared/utils/masks/mask-percentage';
import { useState, type ComponentProps } from 'react';

type InputPercentProps = ComponentProps<'input'> & {
  onChangeHandler?: (value: string) => void;
  minDecimals?: number;
  maxDecimals?: number;
};

export const InputPercent = ({
  value,
  defaultValue,
  maxDecimals,
  ...props
}: InputPercentProps) => {
  const valueFormated = value || defaultValue;
  const [percentValue, setPercentValue] = useState(valueFormated || '');

  return (
    <div className="relative">
      <Input
        {...props}
        id="interestRate"
        value={percentValue}
        onChange={(e) => {
          const value = e.target.value;
          const sanitized = maskPercent(value, maxDecimals);
          setPercentValue(sanitized);
        }}
        className="pr-8"
      />
      <span className="absolute right-3 top-1/2 -translate-y-1/2 text-sm text-muted-foreground pointer-events-none">
        %
      </span>
    </div>
  );
};
