import { useMutation, useQueryClient } from '@tanstack/react-query';

import { ProductService } from '../../domain/services';
import type {
  CreateProductInput,
  UpdateProductInput,
  DeleteProductInput,
  UploadProductImageInput
} from '../../domain/entities';

export const useCreateProduct = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: CreateProductInput) => ProductService.create(input),
    meta: { successMessage: 'Produto criado com sucesso!' },
    onSuccess: (_data, input) => {
      queryClient.invalidateQueries({
        queryKey: ['products', input.establishmentId]
      });
    }
  });
};

export const useUpdateProduct = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: UpdateProductInput) => ProductService.update(input),
    meta: { successMessage: 'Produto atualizado com sucesso!' },
    onSuccess: (_data, input) => {
      queryClient.invalidateQueries({
        queryKey: ['products', input.establishmentId]
      });
    }
  });
};

export const useDeleteProduct = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: DeleteProductInput) => ProductService.delete(input),
    meta: { successMessage: 'Produto removido com sucesso!' },
    onSuccess: (_data, input) => {
      queryClient.invalidateQueries({
        queryKey: ['products', input.establishmentId]
      });
    }
  });
};

export const useUploadProductImage = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (input: UploadProductImageInput) =>
      ProductService.uploadImage(input),
    meta: { successMessage: 'Imagem enviada com sucesso!' },
    onSuccess: (_data, input) => {
      queryClient.invalidateQueries({
        queryKey: ['products', input.establishmentId]
      });
    }
  });
};
