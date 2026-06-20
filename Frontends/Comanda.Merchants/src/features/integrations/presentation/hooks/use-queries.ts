import { useQuery } from '@tanstack/react-query';

import { CredentialService } from '../../domain/services';
import type { Credential } from '../../domain/entities';

export const useCredentials = (establishmentId: string | undefined) => {
  return useQuery<Credential[]>({
    queryKey: ['credentials', establishmentId],
    queryFn: () => CredentialService.list(establishmentId!),
    enabled: !!establishmentId
  });
};
