import { Button } from '@/shared/components/ui/button';
import { Loader2, type LucideIcon } from 'lucide-react';
import type { ComponentProps } from 'react';

interface SubmitingButtonProps extends ComponentProps<typeof Button> {
  label?: string;
  Icon?: LucideIcon;
  showLabel?: boolean;
  state: boolean;
  onClick?: () => void;
}

export const SubmitingButton = ({
  label,
  state,
  showLabel = true,
  Icon,
  ...props
}: SubmitingButtonProps) => {
  return state ? (
    <Button type="submit" disabled {...props}>
      <Loader2 className="mr-0.5 h-3 w-3 animate-spin" />
      {showLabel ? label : null}
    </Button>
  ) : (
    <Button type="submit" {...props}>
      {Icon && <Icon />}
      {label}
    </Button>
  );
};
