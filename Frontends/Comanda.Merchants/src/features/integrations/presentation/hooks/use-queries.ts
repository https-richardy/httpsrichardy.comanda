import { useQuery } from '@tanstack/react-query';

import { IntegrationsAPI } from '../../data/api';
import { IntegrationsMapper } from '../../data/mapper';
import type { Integration } from '../../domain/entities';

export const useGetIntegrations = () => {
  return useQuery<Integration[]>({
    queryKey: ['integrations'],
    queryFn: async (): Promise<Integration[]> => {
      const data = await IntegrationsAPI.list();
      return data.map(IntegrationsMapper.toDomain);
    }
  });
};
