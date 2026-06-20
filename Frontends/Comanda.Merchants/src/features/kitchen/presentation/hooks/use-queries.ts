import { useQueries } from '@tanstack/react-query';

import { OrderService } from '@/features/orders/domain/services';
import type { Order } from '../../domain/entities';

export const useKitchenOrders = () => {
  const results = useQueries({
    queries: [
      {
        queryKey: ['orders', { status: 'Confirmed' }],
        queryFn: () => OrderService.list({ status: 'Confirmed', pageSize: 50 })
      },
      {
        queryKey: ['orders', { status: 'InPreparation' }],
        queryFn: () => OrderService.list({ status: 'InPreparation', pageSize: 50 })
      }
    ]
  });

  const isLoading = results.some((r) => r.isLoading);
  const isError = results.some((r) => r.isError);

  const confirmed: Order[] = results[0].data?.items ?? [];
  const inPreparation: Order[] = results[1].data?.items ?? [];

  return { confirmed, inPreparation, isLoading, isError };
};
