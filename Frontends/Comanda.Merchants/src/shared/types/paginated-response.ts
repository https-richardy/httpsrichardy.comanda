export interface PaginationMetadata {
  total: number;
  totalPages: number;
  pageNumber: number;
  pageSize: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface PagedResult<T> {
  items: T[];
  pagination: PaginationMetadata;
}
