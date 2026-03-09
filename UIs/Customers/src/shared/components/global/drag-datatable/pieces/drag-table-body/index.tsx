import { TableBody, TableCell, TableRow } from '@/shared/components/ui/table';
import {
  SortableContext,
  verticalListSortingStrategy
} from '@dnd-kit/sortable';

import { useDataTable } from '../../../datatable/hook/usetable';
import { DragTableRow } from '../drag-table-row';

export function DragTableBody() {
  const { table, emptyMessage } = useDataTable();
  const { rows } = table.getRowModel();

  const dataIds = rows.map((row) => row.id);

  return (
    <TableBody>
      {rows.length > 0 ? (
        <SortableContext items={dataIds} strategy={verticalListSortingStrategy}>
          {rows.map((row) => (
            <DragTableRow key={row.id} row={row} />
          ))}
        </SortableContext>
      ) : (
        <TableRow>
          <TableCell
            colSpan={table.getAllColumns().length}
            className="text-muted-foreground h-24 text-center"
          >
            {emptyMessage || 'Nenhum registro disponível.'}
          </TableCell>
        </TableRow>
      )}
    </TableBody>
  );
}
