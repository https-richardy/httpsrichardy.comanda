import { useMutation } from '@tanstack/react-query';
import { authService } from '../services';
import { useAuthStore } from '../store';
import type { LoginCredentials } from '../types';

export const useLogin = () => {
  const setSession = useAuthStore((state) => state.setSession);

  return useMutation({
    mutationFn: (credentials: LoginCredentials) =>
      authService.login(credentials),
    meta: {
      successMessage: 'Login realizado com sucesso! Redirecionando...'
    },
    onSuccess: (session) => {
      console.log(session)
      setSession(session);
    }
  });
};
