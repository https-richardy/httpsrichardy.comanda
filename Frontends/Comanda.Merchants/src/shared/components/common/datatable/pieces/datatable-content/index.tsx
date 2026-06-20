import { useMemo } from 'react';

import { Table } from '@/shared/components/ui/table';

import { useDataTable } from '../../hook/usetable';
import { DataTableBody, MemoizedDataTableBody } from '../datatable-body';
import { DataTableHeader } from '../datatable-header';

export function DataTableContent() {
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
      {columnSizingInfo.isResizingColumn && <MemoizedDataTableBody />}
      {!columnSizingInfo.isResizingColumn && <DataTableBody />}
    </Table>
  );
}
