import {
  useReactTable,
  getCoreRowModel,
  flexRender,
  type ColumnDef
} from '@tanstack/react-table';

import {
  Table,
  TableBody,
  TableRow,
  TableCell
} from '@/shared/components/ui/table';
import { useDataTable } from '../../hook/usetable';

interface TNestedDataTable<TData> {
  data: TData[];

  columns: ColumnDef<TData, any>[];

  parentData?: any;
}

export function NestedDataTable<TData>({
  data,
  columns,
  parentData
}: TNestedDataTable<TData>) {
  const { table: parentTable } = useDataTable();
  const parentColumns = parentTable.getVisibleFlatColumns();

  const nestedTable = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    meta: {
      parentData: parentData
    }
  });

  return (
    <Table>
      <TableBody>
        {nestedTable.getRowModel().rows.map((row, index) => (
          <TableRow key={`nested-row-${row.id}-${index}`}>
            {row.getVisibleCells().map((cell, idx) => {
              const parentCol = parentColumns[idx];

              return (
                <TableCell
                  key={`nested-cell-${cell.id}-${idx}`}
                  style={
                    parentCol
                      ? { width: `calc(var(--th-${parentCol.id}-size) * 1px)` }
                      : undefined
                  }
                >
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </TableCell>
              );
            })}
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
