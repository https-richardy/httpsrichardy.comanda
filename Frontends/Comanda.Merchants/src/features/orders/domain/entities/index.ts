import type { PagedResult } from '@/shared/types/paginated-response';

export type OrderStatus =
  | 'Pending'
  | 'Confirmed'
  | 'InPreparation'
  | 'Ready'
  | 'Finalized'
  | 'Cancelled'
  | 'Failed'
  | 'Refunded'
  | 'Returned';

export type OrderPriority = 'Low' | 'Normal' | 'High';

export type OrderFulfillment = 'Unspecified' | 'Pickup' | 'Delivery';

export interface OrderAddress {
  street: string;
  number: string;
  city: string;
  state: string;
  zipCode: string;
  neighborhood: string;
  complement?: string;
  reference?: string;
}

export interface OrderItem {
  title: string;
  description: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface Order {
  id: string;
  code: string;
  address?: OrderAddress;
  status: OrderStatus;
  priority: OrderPriority;
  fulfillment: OrderFulfillment;
  items: OrderItem[];
  merchantId: string;
  consumerId: string;
}

export interface OrderFilters {
  id?: string;
  code?: string;
  status?: OrderStatus;
  priority?: OrderPriority;
  fulfillment?: OrderFulfillment;
  merchantId?: string;
  consumerId?: string;
  pageNumber?: number;
  pageSize?: number;
  createdAfter?: string;
  createdBefore?: string;
}

export interface CreateOrderItem {
  title: string;
  description: string;
  quantity: number;
  unitPrice: number;
}

export interface CreateOrderInput {
  fulfillment: OrderFulfillment;
  priority: OrderPriority;
  address?: OrderAddress;
  merchantId: string;
  consumerId: string;
  items: CreateOrderItem[];
}

export interface UpdateOrderInput {
  id: string;
  status: OrderStatus;
  priority: OrderPriority;
}

export type PagedOrders = PagedResult<Order>;

