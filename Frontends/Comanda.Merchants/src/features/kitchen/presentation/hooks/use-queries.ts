import { useQuery } from '@tanstack/react-query';

import { KitchenAPI } from '../../data/api';
import { KitchenMapper } from '../../data/mapper';
import type { KitchenItem } from '../../domain/entities';

export const useGetKitchenItems = () => {
  return useQuery<KitchenItem[]>({
    queryKey: ['kitchen-items'],
    queryFn: async (): Promise<KitchenItem[]> => {
      const data = await KitchenAPI.list();
      return data.map(KitchenMapper.toDomain);
    }
  });
};
