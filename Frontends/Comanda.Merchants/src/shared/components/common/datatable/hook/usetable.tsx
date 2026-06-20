'use client';

import { createContext, type ReactNode, useContext } from 'react';
import type { Row, Table } from '@tanstack/react-table';

type DataTableContextType<TData> = {
  table: Table<TData>;
  emptyMessage?: string;
  renderSubRow?: (row: Row<TData>, index: number) => ReactNode;
};

export const DataTableContext = createContext<
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  DataTableContextType<any> | undefined
>(undefined);

export const useDataTable = <TData,>() => {
  const context = useContext(
    DataTableContext as React.Context<DataTableContextType<TData> | undefined>
  );

  if (!context) {
    throw new Error('useDataTable must be used within a DataTable');
  }

  return context;
};
