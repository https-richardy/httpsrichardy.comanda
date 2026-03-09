import type { ColumnDef, Row } from '@tanstack/react-table';
import type { ReactNode } from 'react';

export interface DragTableProps<TData> {
  data: TData[];
  columns: ColumnDef<TData>[];
  onReorder: (newData: TData[]) => void;
  renderDragHandle?: (row: Row<TData>) => ReactNode;
  emptyMessage?: string;
  children?: ReactNode;
}
