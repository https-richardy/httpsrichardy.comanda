import {
  ChefHat,
  CreditCard,
  Package,
  PlugZap,
  ShoppingBag
} from 'lucide-react';

export type AppItem = {
  name: string;
  href: string;
  icon?: any;
};

export const apps: AppItem[] = [
  {
    name: 'Pedidos',
    href: '/pedidos',
    icon: ShoppingBag
  },
  {
    name: 'Cozinha',
    href: '/cozinha',
    icon: ChefHat
  },
  {
    name: 'Pagamento',
    href: '/pagamento',
    icon: CreditCard
  },
  {
    name: 'Produtos',
    href: '/produtos',
    icon: Package
  },
  {
    name: 'Integrações',
    href: '/integracoes',
    icon: PlugZap
  }
];
