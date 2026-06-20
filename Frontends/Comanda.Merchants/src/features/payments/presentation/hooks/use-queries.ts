import { useQuery } from '@tanstack/react-query';

import { PaymentsAPI } from '../../data/api';
import { PaymentsMapper } from '../../data/mapper';
import type { Payment } from '../../domain/entities';

export const useGetPayments = () => {
  return useQuery<Payment[]>({
    queryKey: ['payments'],
    queryFn: async (): Promise<Payment[]> => {
      const data = await PaymentsAPI.list();
      return data.map(PaymentsMapper.toDomain);
    }
  });
};
