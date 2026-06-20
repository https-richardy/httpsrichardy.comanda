import { cn } from '@/shared/utils';

import type { IBoardColumnFilters } from '../../types';

export function BoardColumnFilters({
  children,
  className
}: IBoardColumnFilters) {
  return (
    <div className={cn('px-3 py-2 border-b bg-muted/20', className)}>
      {children}
    </div>
  );
}
