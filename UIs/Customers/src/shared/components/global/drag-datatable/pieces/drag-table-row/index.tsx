import { TableCell, TableRow } from '@/shared/components/ui/table';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { flexRender, type Row } from '@tanstack/react-table';
import { type CSSProperties } from 'react';

interface DragTableRowProps<TData> {
  row: Row<TData>;
}

export function DragTableRow<TData>({ row }: DragTableRowProps<TData>) {
  const { transform, transition, setNodeRef, isDragging } = useSortable({
    id: row.id
  });

  const style: CSSProperties = {
    transform: CSS.Transform.toString(transform),
    transition: transition,
    position: 'relative',
    zIndex: isDragging ? 10 : 0,
    opacity: isDragging ? 0.6 : 1
  };

  return (
    <TableRow
      ref={setNodeRef}
      style={style}
      data-state={row.getIsSelected() && 'selected'}
      data-dragging={isDragging}
      className="data-[dragging=true]:bg-muted"
    >
      {row.getVisibleCells().map((cell) => (
        <TableCell
          key={cell.id}
          style={{
            width: `var(--col-${cell.column.id}-size)`
          }}
        >
          {flexRender(cell.column.columnDef.cell, cell.getContext())}
        </TableCell>
      ))}
    </TableRow>
  );
}
