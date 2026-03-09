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
          {
            path: 'kitchen',
            lazy: async () => {
              const { KitchenPage } = await import('@/features/kitchen/pages');
              return { Component: KitchenPage };
            }
          },
          {
            path: 'payments',
            lazy: async () => {
              const { PaymentsPage } = await import('@/features/payments/pages');
              return { Component: PaymentsPage };
            }
          },
          {
            path: 'orders',
            lazy: async () => {
              const { OrdersPage } = await import('@/features/orders/pages');
              return { Component: OrdersPage };
            }
          },
          {
            path: 'settings',
            lazy: async () => {
              const { SettingsPage } = await import('@/features/settings/pages');
              return { Component: SettingsPage };
            }
          }
        ]
      }
    ]
  }
]);

export function AppRouters() {
  return <RouterProvider router={routers} />;
}
