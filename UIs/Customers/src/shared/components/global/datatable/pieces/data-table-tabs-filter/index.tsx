import { Tabs, TabsList, TabsTrigger } from '@/shared/components/ui/tabs';
import { useDataTable } from '../../hook/usetable';
import { useState } from 'react';

export interface IDataTableTabFilter<T> {
  column: string;
  options: {
    value: T;
    label: string;
  }[];
  initialValue: T;
  onValueChange?: (value: T) => void;
}

export function DataTableTabFilter<T>({
  column,
  options,
  initialValue,
  onValueChange
}: IDataTableTabFilter<T>) {
  const { table } = useDataTable();
  const [currentValue, setCurrentValue] = useState<T>(initialValue);
  const tableColumn = table.getColumn(column);

  const handleValueChange = (newValue: T) => {
    tableColumn?.setFilterValue(newValue);
    setCurrentValue(newValue);
    onValueChange?.(newValue);
  };

  return (
    <Tabs
      value={currentValue as string}
      onValueChange={handleValueChange as (value: string) => void}
      className="w-auto"
    >
      <TabsList className="grid w-full grid-flow-col justify-start h-9">
        {options.map(({ value, label }) => {
          return (
            <TabsTrigger
              key={`filter-tab-${column}-${value}`}
              value={value as string}
              className={`px-3 ${currentValue === value && 'text-red-700!'}`}
            >
              {label}
            </TabsTrigger>
          );
        })}
      </TabsList>
    </Tabs>
  );
}
