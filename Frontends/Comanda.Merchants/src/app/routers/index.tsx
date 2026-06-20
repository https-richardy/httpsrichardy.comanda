import { createBrowserRouter, Navigate, RouterProvider } from 'react-router';

import { PublicLayout } from '../layouts/public';

import { AuthGuard } from './guards/auth-guard';
import { GuestGuard } from './guards/guest-guard';

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
              const { LoginForm } = await import('@/features/auth');
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
            element: <Navigate to="/pedidos" />
          },
          {
            path: 'pedidos',
            lazy: async () => {
              const { OrdersPage } = await import('@/features/orders');
              return { Component: OrdersPage };
            }
          },
          {
            path: 'cozinha',
            lazy: async () => {
              const { KitchenPage } = await import('@/features/kitchen');
              return { Component: KitchenPage };
            }
          },
          {
            path: 'pagamento',
            lazy: async () => {
              const { PaymentsPage } = await import('@/features/payments');
              return { Component: PaymentsPage };
            }
          },
          {
            path: 'produtos',
            lazy: async () => {
              const { ProductsPage } = await import('@/features/products');
              return { Component: ProductsPage };
            }
          },
          {
            path: 'integracoes',
            lazy: async () => {
              const { IntegrationsPage } = await import(
                '@/features/integrations'
              );
              return { Component: IntegrationsPage };
            }
          },
          {
            path: '*',
            element: <Navigate to="/pedidos" />
          }
        ]
      }
    ]
  }
]);

export function AppRouters() {
  return <RouterProvider router={routers} />;
}
