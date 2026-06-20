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

export interface OrderAddressDTO {
  street: string;
  number: string;
  city: string;
  state: string;
  zipCode: string;
  neighborhood: string;
  complement?: string;
  reference?: string;
}

export interface OrderItemDTO {
  title: string;
  description: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface OrderMetadataDTO {
  merchantId: string;
  consumerId: string;
}

export interface OrderDTO {
  identifier: string;
  code: string;
  address?: OrderAddressDTO;
  status: OrderStatus;
  priority: OrderPriority;
  fulfillment: OrderFulfillment;
  items: OrderItemDTO[];
  metadata: OrderMetadataDTO;
}

export interface OrderFiltersDTO {
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

export interface CreateOrderItemDTO {
  title: string;
  description: string;
  quantity: number;
  unitPrice: number;
}

export interface CreateOrderDTO {
  fulfillment: OrderFulfillment;
  priority: OrderPriority;
  address?: OrderAddressDTO;
  metadata: OrderMetadataDTO;
  items: CreateOrderItemDTO[];
}

export interface UpdateOrderDTO {
  status: OrderStatus;
  priority: OrderPriority;
}

