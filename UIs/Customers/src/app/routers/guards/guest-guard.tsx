import { Outlet, Navigate } from 'react-router';
import { useAuthStore } from '@/features/auth/store';

export const GuestGuard = () => {
  const status = useAuthStore((state) => state.status);

  if (status === 'loading' || status === 'idle') {
    return null;
  }

  if (status === 'authenticated') {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
};
