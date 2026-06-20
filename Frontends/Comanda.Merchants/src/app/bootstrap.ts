import { useAuthStore } from '@/features/auth';

import { setupHttpClientInterceptors } from '@/infra/api/gateway-api';

export const bootstrapApp = () => {
  const getToken = () => useAuthStore.getState().token;

  const onUnauthorized = () => {
    const { expiresAt } = useAuthStore.getState();

    if (!expiresAt) {
      window.dispatchEvent(new CustomEvent('on-session-expired'));
      useAuthStore.getState().logout();
      return;
    }

    const tokenExpiresAt = new Date(expiresAt);
    const isExpired = tokenExpiresAt < new Date();

    if (isExpired) {
      window.dispatchEvent(new CustomEvent('on-session-expired'));
      useAuthStore.getState().logout();
    }
  };

  setupHttpClientInterceptors(getToken, onUnauthorized);
};
