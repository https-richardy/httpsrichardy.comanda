import {
  ChefHat,
  CreditCard,
  Package,
  PlugZap,
  ShoppingBag
} from 'lucide-react';

import type { NavItem } from './nav-items';

export const Navlinks: NavItem[] = [
  {
    title: 'Pedidos',
    url: '/pedidos',
    icon: ShoppingBag
  },
  {
    title: 'Cozinha',
    url: '/cozinha',
    icon: ChefHat
  },
  {
    title: 'Pagamento',
    url: '/pagamento',
    icon: CreditCard
  },
  {
    title: 'Produtos',
    url: '/produtos',
    icon: Package
  },
  {
    title: 'Integrações',
    url: '/integracoes',
    icon: PlugZap
  }
];
