import { useEffect } from 'react';
import { Outlet, useLocation, Navigate } from 'react-router';
import { useAuthStore } from '@/features/auth/store';

export const AuthGuard = () => {
  const location = useLocation();
  const status = useAuthStore((state) => state.status);
  const checkSession = useAuthStore((state) => state.checkSession);

  useEffect(() => {
    checkSession();
  }, [checkSession]);

  if (status === 'loading' || status === 'idle') {
    return null;
  }

  if (status !== 'authenticated') {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return <Outlet />;
};
