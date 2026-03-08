export type PaginatedResponse<T> = {
  pageNumber: number;
  pageSize: number;
  totalResults: number;
  totalPages: number;
  results: T[];
};
