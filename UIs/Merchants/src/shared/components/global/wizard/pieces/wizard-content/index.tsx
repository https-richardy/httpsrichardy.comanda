import type { ComponentProps } from 'react';
import { useWizard } from '../../hooks';
import { Card } from '@/shared/components/ui/card';
import { cn } from '@/shared/utils';

export function WizardContent({ className, ...props }: ComponentProps<'div'>) {
  const { state } = useWizard();

  return (
    <Card
      className={cn('p-4 md:p-6 h-full overflow-y-auto', className)}
      {...props}
    >
      {state.currentStep?.component}
    </Card>
  );
}
