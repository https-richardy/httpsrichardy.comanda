import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import type { AuthSession } from '../types';
import { authService } from '../services';

interface AuthState extends AuthSession {
  setSession: (session: AuthSession) => void;
  logout: () => void;
  checkSession: () => void;
}

const initialState: AuthSession = {
  token: null,
  user: null,
  status: 'unauthenticated',
  expiresAt: null
};

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      ...initialState,

      setSession: (session) => {
        console.log(session)
        set(session);
      },

      logout: () => {
        authService.logout();
        set(initialState);
      },

      checkSession: () => {
        const { expiresAt, token } = get();
        console.log(expiresAt)
        console.log(Date.now())

        if (!token || !expiresAt) return;
        
      }
    }),
    {
      name: 'template-app',
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
