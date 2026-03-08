import { useMemo } from 'react';
import { Table } from '@/shared/components/ui/table';
import { useDataTable } from '../../../datatable/hook/usetable';
import { DataTableHeader } from '../../../datatable/pieces/datatable-header';
import { DragTableBody } from '../drag-table-body';

export function DragTableContent() {
  const { table } = useDataTable();
  const { columnSizingInfo, columnSizing } = table.getState();

  const colSizeVariables = useMemo(
    () =>
      table.getFlatHeaders().reduce<Record<string, number>>(
        (acc, header) => ({
          ...acc,
          [`--th-${header.id}-size`]: header.getSize(),
          [`--col-${header.column.id}-size`]: header.column.getSize()
        }),
        {}
      ),
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [columnSizing, columnSizingInfo, table.getFlatHeaders]
  );

  return (
    <Table style={colSizeVariables}>
      <DataTableHeader />
      <DragTableBody />
    </Table>
  );
}
