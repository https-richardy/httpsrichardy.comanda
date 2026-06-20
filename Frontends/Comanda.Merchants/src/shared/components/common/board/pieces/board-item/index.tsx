import { cn } from '@/shared/utils';

import { useBoard } from '../../hook/use-board';
import type { IBoardItem } from '../../types';

export function BoardItem({
  id,
  draggable = false,
  children,
  className
}: IBoardItem) {
  const { getItemProps } = useBoard();
  const { onDragStart, onDragEnd } = getItemProps(id);

  return (
    <div
      draggable={draggable}
      onDragStart={onDragStart}
      onDragEnd={onDragEnd}
      className={cn(
        'cursor-grab active:cursor-grabbing select-none transition-opacity',
        'active:opacity-60',
        className
      )}
    >
      {children}
    </div>
  );
}
