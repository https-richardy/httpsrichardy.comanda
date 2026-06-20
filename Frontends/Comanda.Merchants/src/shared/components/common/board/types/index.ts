import type { ReactNode } from 'react';

export interface BoardContextValue {
  draggingId: string | null;
  setDraggingId: (id: string | null) => void;
}

export interface IBoardColumn {
  id: string;
  onDrop: (itemId: string, columnId: string) => void;
  header?: ReactNode;
  children?: ReactNode;
  isDropDisabled?: boolean;
  className?: string;
}

export interface IBoardItem {
  id: string;
  draggable?: boolean;
  children?: ReactNode;
  className?: string;
}

export interface IBoardColumnFilters {
  children: ReactNode;
  className?: string;
}

export interface IBoardColumnHeader {
  children: ReactNode;
  className?: string;
}

export interface IColumnFilterOption<TFilterOptions> {
  label: string;
  value: Partial<TFilterOptions>;
}

export interface IBoardColumnConfig<TColumnStages, TFilterOptions> {
  id: TColumnStages;
  color: string;
  title: string;
  filters?: IColumnFilterOption<TFilterOptions>[];
  canCreateLead?: boolean;
}

export interface IBoardConfig<TColumnStages, TFilterOptions> {
  createItemComponent: ReactNode;
  columns: IBoardColumnConfig<TColumnStages, TFilterOptions>[];
}
