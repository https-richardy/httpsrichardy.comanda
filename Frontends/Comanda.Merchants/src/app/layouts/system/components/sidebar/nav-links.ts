import {
  ChefHat,
  CreditCard,
  MessageCircle,
  Package,
  PlugZap,
  Settings,
  ShoppingBag,
  Tag
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
    title: 'Comandas',
    url: '/comandas',
    icon: Tag
  },
  {
    title: 'Integrações',
    url: '/integracoes',
    icon: PlugZap
  },
  {
    title: 'Suporte',
    url: '/suporte',
    icon: MessageCircle
  },
  {
    title: 'Configurações',
    url: '/configuracoes',
    icon: Settings
  }
];
