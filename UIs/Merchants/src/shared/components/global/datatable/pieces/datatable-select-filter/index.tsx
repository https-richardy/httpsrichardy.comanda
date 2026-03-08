import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/shared/components/ui/select';
import { useDataTable } from '../../hook/usetable';
import { cn } from '@/shared/utils';

export interface DataTableSelectFilter {
  placeholder: string;
  column: string;
  className?: string;
  options: {
    value: string | number | boolean;
    label: string;
  }[];
}

export function DataTableSelectFilter({
  placeholder,
  column,
  className,
  options
}: DataTableSelectFilter) {
  const { table } = useDataTable();
  const tableColumn = table.getColumn(column);
  const filterValue = tableColumn?.getFilterValue() as string;
  const currentValue = filterValue ?? 'all';

  const handleOnChangeValue = (newValue: string) => {
    if (newValue === 'all') {
      tableColumn?.setFilterValue(undefined);

      return;
    }

    tableColumn?.setFilterValue(newValue);
  };

  return (
    <Select
      value={currentValue}
      onValueChange={(value) => handleOnChangeValue(value)}
    >
      <SelectTrigger
        className={cn('w-full sm:w-[180px]', className)}
        size="custom"
      >
        <SelectValue placeholder={placeholder} />
      </SelectTrigger>
      <SelectContent>
        {options.map(({ value, label }) => (
          <SelectItem
            key={`filter-${column}-option-${value}`}
            value={value.toString()}
          >
            {label}
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  );
}
