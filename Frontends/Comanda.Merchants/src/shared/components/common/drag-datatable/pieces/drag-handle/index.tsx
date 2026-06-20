import { useSortable } from '@dnd-kit/sortable';
import { GripVertical } from 'lucide-react';

import { Button } from '@/shared/components/ui/button';

export function DragHandle({ id }: { id: string | number }) {
  const { attributes, listeners } = useSortable({ id });

  return (
    <Button
      {...attributes}
      {...listeners}
      variant="ghost"
      size="icon"
      className="text-muted-foreground size-7 hover:bg-transparent cursor-grab active:cursor-grabbing"
    >
      <GripVertical className="size-4" />
      <span className="sr-only">Drag to reorder</span>
    </Button>
  );
}
