import type { ReactNode } from 'react';

import { cn } from '@/shared/utils';

import { BoardProvider } from '../../hook/use-board';

interface IBoardProps {
  children: ReactNode;
  className?: string;
}

export function Board({ children, className }: IBoardProps) {
  return (
    <BoardProvider>
      <div
        className={cn(
          'flex gap-4 overflow-x-auto pb-4 min-h-[calc(100vh-12rem)] items-start',
          className
        )}
      >
        {children}
      </div>
    </BoardProvider>
  );
}
