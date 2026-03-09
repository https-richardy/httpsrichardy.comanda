import { type ReactNode } from 'react';
import { useReactTable, type TableOptions } from '@tanstack/react-table';
import {
  closestCenter,
  DndContext,
  type DragEndEvent,
  KeyboardSensor,
  MouseSensor,
  TouchSensor,
  useSensor,
  useSensors
} from '@dnd-kit/core';
import { restrictToVerticalAxis } from '@dnd-kit/modifiers';
import { DataTableContext } from '../../../datatable/hook/usetable';

export interface IDragTable<TData> {
  tableOptions: TableOptions<TData>;
  onDragEnd: (event: DragEndEvent) => void;
  emptyMessage?: string;
  children?: ReactNode;
}

export function DragTable<TData>({
  tableOptions,
  onDragEnd,
  emptyMessage,
  children
}: IDragTable<TData>) {
  const table = useReactTable(tableOptions);

  const sensors = useSensors(
    useSensor(MouseSensor, {}),
    useSensor(TouchSensor, {}),
    useSensor(KeyboardSensor, {})
  );

  return (
    <DndContext
      collisionDetection={closestCenter}
      modifiers={[restrictToVerticalAxis]}
      onDragEnd={onDragEnd}
      sensors={sensors}
    >
      <DataTableContext.Provider value={{ table, emptyMessage }}>
        {children}
      </DataTableContext.Provider>
    </DndContext>
  );
}
