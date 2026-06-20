import type { ReactNode } from 'react';

import { cn } from '@/shared/utils';

import { useBoardColumn } from './use-board-column';

interface IBoardColumnProps {
  id: string;
  onDrop: (itemId: string, columnId: string) => void;
  children: ReactNode;
  isDropDisabled?: boolean;
  className?: string;
}

export function BoardColumn({
  id,
  onDrop,
  children,
  isDropDisabled = false,
  className
}: IBoardColumnProps) {
  const { dropzoneProps, isActiveDropTarget } = useBoardColumn({
    id,
    onDrop,
    isDropDisabled
  });

  return (
    <div
      {...dropzoneProps}
      className={cn(
        'flex flex-col w-72 min-w-[18rem] flex-shrink-0 rounded-lg border bg-muted/10 transition-colors duration-150',
        isActiveDropTarget &&
          'border-primary ring-2 ring-primary/20 bg-primary/5',
        isDropDisabled && 'opacity-60 cursor-not-allowed',
        className
      )}
    >
      <div className="flex flex-col gap-0 flex-1 overflow-y-auto h-[calc(100vh-16rem)]">
        {children}
      </div>
    </div>
  );
}
