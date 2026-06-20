import { useQuery } from '@tanstack/react-query';

import { OrdersAPI } from '../../data/api';
import { OrdersMapper } from '../../data/mapper';
import type { Order } from '../../domain/entities';

export const useGetOrders = () => {
  return useQuery<Order[]>({
    queryKey: ['orders'],
    queryFn: async (): Promise<Order[]> => {
      const data = await OrdersAPI.list();
      return data.map(OrdersMapper.toDomain);
    }
  });
};
