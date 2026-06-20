import { useQuery } from '@tanstack/react-query';

import { OrderService } from '../../domain/services';
import type { OrderFilters, PagedOrders } from '../../domain/entities';

export const useOrders = (filters?: OrderFilters) => {
  return useQuery<PagedOrders>({
    queryKey: ['orders', filters],
    queryFn: () => OrderService.list(filters)
  });
};
