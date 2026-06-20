import { QueryClientProvider } from '@tanstack/react-query';
import type { ReactNode } from 'react';
import { useEffect } from 'react';

import { queryClient } from '../config/react-query';
import { ThemeProvider } from '../theme/theme-provider';
import { useAuthStore } from '@/features/auth';

export const AppProviders = ({ children }: { children: ReactNode }) => {
  // Dev helper: create a mock authenticated session when running in dev
  // so developers can access the app without real federation integration.
  useEffect(() => {
    if (!import.meta.env.DEV) return;

    try {
      const storageKey = 'comanda-auth';
      const has = localStorage.getItem(storageKey);

      if (!has) {
        const now = Date.now();
        const mock = {
          token: 'dev-token',
          expiresAt: now + 1000 * 60 * 60 * 24, // 24h
          user: {
            id: 'dev-user',
            email: 'dev@comanda.local',
            name: 'Dev User',
            firstName: 'Dev',
            lastName: 'User',
            roles: ['admin']
          },
          status: 'authenticated'
        } as any;

        useAuthStore.getState().setSession(mock);
      }
    } catch (e) {
      // ignore
    }
  }, []);

  return (
    <ThemeProvider>
      <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
    </ThemeProvider>
  );
};
