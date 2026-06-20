import { useQuery } from '@tanstack/react-query';

import { PaymentService } from '../../domain/services';
import type { PaymentFilters, PagedPayments } from '../../domain/entities';

export const usePayments = (filters?: PaymentFilters) => {
  return useQuery<PagedPayments>({
    queryKey: ['payments', filters],
    queryFn: () => PaymentService.list(filters)
  });
};
