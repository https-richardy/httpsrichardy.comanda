import { Navigate, Outlet, useLocation } from 'react-router';

import { useAuthGuard } from '@/features/auth';

export const AuthGuard = () => {
  const location = useLocation();
  const { status } = useAuthGuard();

  if (status === 'loading' || status === 'idle') {
    return null;
  }

  if (status !== 'authenticated') {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return <Outlet />;
};
