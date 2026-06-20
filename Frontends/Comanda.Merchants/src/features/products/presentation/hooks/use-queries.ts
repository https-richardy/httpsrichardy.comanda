import { useQuery } from '@tanstack/react-query';

import { useAuthStore } from '@/features/auth/presentation/stores';
import { EstablishmentService, ProductService } from '../../domain/services';
import type { ProductFilters, PagedProducts, Establishment } from '../../domain/entities';

export const useCurrentEstablishment = () => {
  const userId = useAuthStore((s) => s.user?.id);

  return useQuery<Establishment | null>({
    queryKey: ['establishments', 'current', userId],
    queryFn: async () => {
      if (!userId) return null;
      const result = await EstablishmentService.list({ ownerId: userId, pageSize: 1 });
      return result.items[0] ?? null;
    },
    enabled: !!userId
  });
};

export const useProducts = (
  establishmentId: string | undefined,
  filters?: ProductFilters
) => {
  return useQuery<PagedProducts>({
    queryKey: ['products', establishmentId, filters],
    queryFn: () => ProductService.list(establishmentId!, filters),
    enabled: !!establishmentId
  });
};
