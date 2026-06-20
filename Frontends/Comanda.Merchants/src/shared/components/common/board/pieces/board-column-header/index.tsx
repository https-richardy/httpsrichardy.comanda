import { cn } from '@/shared/utils';

import type { IBoardColumnHeader } from '../../types';

export function BoardColumnHeader({ children, className }: IBoardColumnHeader) {
  return (
    <div
      className={cn(
        'flex flex-col items-center justify-center px-3 py-2 border-b',
        className
      )}
    >
      {children}
    </div>
  );
}
