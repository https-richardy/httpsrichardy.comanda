import { Input } from '@/shared/components/ui/input';
import { useDataTable } from '../../hook/usetable';
import { Search } from 'lucide-react';
import { useMemo, useState } from 'react';
import { cn, debounce } from '@/shared/utils';

export interface IDataTableTextFilter {
  placeholder: string;
  column?: string;
  className?: string;
}

export function DataTableTextFilter({
  placeholder,
  column,
  className
}: IDataTableTextFilter) {
  const { table } = useDataTable();
  const [localValue, setLocalValue] = useState<string>(
    () =>
      ((column
        ? table.getColumn(column)?.getFilterValue()
        : table.getState().globalFilter) as string) ?? ''
  );

  const updateTableFilter = useMemo(
    () =>
      debounce((value: string) => {
        if (column) {
          table.getColumn(column)?.setFilterValue(value);
        } else {
          table.setGlobalFilter(value);
        }
      }, 1000),
    [table, column]
  );

  const onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    setLocalValue(newValue);
    updateTableFilter(newValue);
  };

  return (
    <div className={cn('flex-1 relative min-w-[200px]', className)}>
      <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
      <Input
        placeholder={placeholder}
        value={localValue}
        onChange={onChange}
        className="pl-9"
      />
    </div>
  );
}
