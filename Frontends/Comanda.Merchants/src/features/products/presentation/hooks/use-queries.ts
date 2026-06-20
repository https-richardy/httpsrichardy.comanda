import { useQuery } from '@tanstack/react-query';

import { ProductsAPI } from '../../data/api';
import { ProductsMapper } from '../../data/mapper';
import type { Product } from '../../domain/entities';

export const useGetProducts = () => {
  return useQuery<Product[]>({
    queryKey: ['products'],
    queryFn: async (): Promise<Product[]> => {
      const data = await ProductsAPI.list();
      return data.map(ProductsMapper.toDomain);
    }
  });
};
