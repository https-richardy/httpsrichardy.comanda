import { MutationCache, QueryClient } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { toast } from 'sonner';

export const queryClient = new QueryClient({
  mutationCache: new MutationCache({
    onError: (error, _variables, _context, mutation) => {
      if (mutation.meta?.suppressErrorToast) return;

      const customMessage = mutation.meta?.errorMessage;
      const backendMessage =
        error instanceof AxiosError ? error.message : 'Erro desconhecido';

      toast.error(customMessage || `Erro na operação: ${backendMessage}`);
    },
    onSuccess: (_data, _variables, _context, mutation) => {
      if (mutation.meta?.successMessage) {
        toast.success(mutation.meta.successMessage);
      }
    }
  }),

  defaultOptions: {
    queries: {
      retry: false,
      refetchOnWindowFocus: false
    }
  }
});
