import { createBrowserRouter, RouterProvider } from 'react-router';
import { AuthGuard } from './guards/auth-guard';
import { GuestGuard } from './guards/guest-guard';
import { PublicLayout } from '../layouts/public';

export const routers = createBrowserRouter([
  {
    element: <GuestGuard />,
    children: [
      {
        element: <PublicLayout />,
        children: [
          {
            path: '/login',
            lazy: async () => {
              const { LoginForm } = await import(
                '@/features/auth/components/login-form'
              );
              return { Component: LoginForm };
            }
          }
        ]
      }
    ]
  },
  {
    element: <AuthGuard />,
    children: [
      {
        path: '/',
        lazy: async () => {
          const { SystemLayout } = await import('@/app/layouts/system');
          return { Component: SystemLayout };
        },
        children: [
          {
            index: true,
            lazy: async () => {
              const { DashboardPage } = await import(
                '@/features/dashboard/pages'
              );
              return { Component: DashboardPage };
            }
          },
          {
            path: 'dashboard',
            lazy: async () => {
              const { DashboardPage } = await import(
                '@/features/dashboard/pages'
              );
              return { Component: DashboardPage };
            }
          },
        ]
      }
    ]
  }
]);

export function AppRouters() {
  return <RouterProvider router={routers} />;
}
