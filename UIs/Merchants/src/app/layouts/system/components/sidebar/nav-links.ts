import {
  ChefHat,
  ClipboardList,
  CreditCard,
  Home,
  Settings
} from 'lucide-react';
import type { NavItem } from './nav-items';

export const Navlinks: NavItem[] = [
  {
    title: 'Dashboard',
    url: '/dashboard',
    icon: Home
  },
  {
    title: 'Cozinha',
    url: '/kitchen',
    icon: ChefHat
  },
  {
    title: 'Pagamentos',
    url: '/payments',
    icon: CreditCard
  },
  {
    title: 'Pedidos',
    url: '/orders',
    icon: ClipboardList
  },
  {
    title: 'Configurações',
    url: '/settings',
    icon: Settings
  }
];
