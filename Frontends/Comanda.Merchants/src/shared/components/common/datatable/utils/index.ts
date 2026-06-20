import type { Row } from '@tanstack/react-table';

export const dateRangeFilter = (
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  row: Row<any>,
  columnId: string,
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  value: any
) => {
  const dateValue = row.getValue(columnId);
  const { from, to } = value || {};

  if (!from && !to) return true;
  if (!dateValue) return false;

  const rowDate = new Date(dateValue as string | number);

  if (to) {
    return rowDate >= from && rowDate <= to;
  }

  return rowDate >= from;
};
