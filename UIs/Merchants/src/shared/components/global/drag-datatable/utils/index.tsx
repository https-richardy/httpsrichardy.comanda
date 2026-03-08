import { type ColumnDef } from '@tanstack/react-table';
import { DragHandle } from '../pieces/drag-handle';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger
} from '@/shared/components/ui/tooltip';

/**
 * Prepend the drag handle column to the existing columns definition.
 * @param columns The original columns definition.
 * @returns A new array of columns with the drag handle as the first column.
 */
export function getDragTableColumns<TData>(
  columns: ColumnDef<TData>[]
): ColumnDef<TData>[] {
  const dragColumn: ColumnDef<TData> = {
    id: 'drag',
    header: () => null,
    cell: ({ row }) => (
      <Tooltip>
        <TooltipTrigger asChild>
          <DragHandle id={row.id} />
        </TooltipTrigger>
        <TooltipContent>
          <p>Arraste para alterar a prioridade do banco</p>
        </TooltipContent>
      </Tooltip>
    ),
    size: 50,
    enableSorting: false,
    enableHiding: false,
    enableResizing: false
  };

  return [dragColumn, ...columns];
}
