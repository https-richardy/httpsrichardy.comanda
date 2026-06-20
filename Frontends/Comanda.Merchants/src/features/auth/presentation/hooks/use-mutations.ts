import { useMutation, useQueryClient } from '@tanstack/react-query';

import { authService } from '../../domain/services';
import { useAuthStore } from '../stores';

export const useLogin = () => {
  const { setSession } = useAuthStore();

  return useMutation({
    mutationFn: authService.login,
    meta: {
      successMessage: 'Login realizado com sucesso!'
    },
    onSuccess: (session) => {
      setSession(session);
    }
  });
};

export const useLogout = () => {
  const { logout } = useAuthStore();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: authService.logout,
    meta: {
      successMessage: 'Logout realizado com sucesso'
    },
    onSuccess: () => {
      queryClient.clear();
      logout();
    }
  });
};
