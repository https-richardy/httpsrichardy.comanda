import { create } from 'zustand';
import { createJSONStorage, persist } from 'zustand/middleware';

import type { AuthSession } from '../../domain/entities';

interface AuthState extends AuthSession {
  setSession: (session: AuthSession) => void;
  logout: () => void;
}

const initialState = {
  status: 'unauthenticated',
  token: null
} as AuthState;

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      ...initialState,

      setSession: (session) => {
        set(session);
      },

      logout: () => {
        set(initialState);
      }
    }),
    {
      name: 'comanda-auth',
      storage: createJSONStorage(() => localStorage),
      partialize: (state) => ({
        token: state.token,
        user: state.user,
        expiresAt: state.expiresAt,
        status: state.token ? 'authenticated' : 'unauthenticated'
      })
    }
  )
);
