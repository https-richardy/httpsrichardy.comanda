import { useMutation, useQueryClient } from '@tanstack/react-query';

import { PaymentService } from '../../domain/services';
import type {
  CreateOfflinePaymentInput,
  CreateOnlinePaymentInput,
  UpdatePaymentStatusInput
} from '../../domain/entities';

export const useCreateOfflinePayment = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: CreateOfflinePaymentInput) =>
      PaymentService.createOffline(input),
    meta: { successMessage: 'Pagamento registrado com sucesso!' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['payments'] });
    }
  });
};

export const useCreateOnlinePayment = () => {
  return useMutation({
    mutationFn: (input: CreateOnlinePaymentInput) =>
      PaymentService.createOnline(input),
    meta: { successMessage: 'Cobrança criada com sucesso!' }
  });
};

export const useUpdatePaymentStatus = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: UpdatePaymentStatusInput) =>
      PaymentService.updateStatus(input),
    meta: { successMessage: 'Status do pagamento atualizado!' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['payments'] });
    }
  });
};
