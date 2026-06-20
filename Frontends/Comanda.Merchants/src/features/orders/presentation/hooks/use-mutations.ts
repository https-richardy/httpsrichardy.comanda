import { useMutation, useQueryClient } from '@tanstack/react-query';

import { OrderService } from '../../domain/services';
import type { CreateOrderInput, UpdateOrderInput } from '../../domain/entities';

export const useCreateOrder = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: CreateOrderInput) => OrderService.create(input),
    meta: { successMessage: 'Pedido criado com sucesso!' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orders'] });
    }
  });
};

export const useUpdateOrder = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: UpdateOrderInput) => OrderService.update(input),
    meta: { successMessage: 'Pedido atualizado com sucesso!' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orders'] });
    }
  });
};
