import { useMutation, useQueryClient } from '@tanstack/react-query';

import { CredentialService } from '../../domain/services';
import type { CreateCredentialInput, UpdateCredentialInput } from '../../domain/entities';

export const useCreateCredential = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: CreateCredentialInput) => CredentialService.create(input),
    meta: { successMessage: 'Integração configurada com sucesso!' },
    onSuccess: (_data, input) => {
      queryClient.invalidateQueries({
        queryKey: ['credentials', input.establishmentId]
      });
    }
  });
};

export const useUpdateCredential = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: UpdateCredentialInput) => CredentialService.update(input),
    meta: { successMessage: 'Integração atualizada com sucesso!' },
    onSuccess: (_data, input) => {
      queryClient.invalidateQueries({
        queryKey: ['credentials', input.establishmentId]
      });
    }
  });
};
