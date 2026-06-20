import type { PagedResult } from '@/shared/types/paginated-response';

export type PaymentStatus = 'Pending' | 'Paid' | 'Refunded' | 'Failed';

export type PaymentMethod = 'Unspecified' | 'Pix' | 'Card' | 'Cash';

export interface PaymentPayer {
  id: string;
  name: string;
}

export interface Payment {
  id: string;
  reference: string;
  amount: number;
  payer: PaymentPayer;
  status: PaymentStatus;
  method: PaymentMethod;
}

export interface PaymentFilters {
  id?: string;
  payerId?: string;
  externalId?: string;
  referenceId?: string;
  status?: PaymentStatus;
  method?: PaymentMethod;
  pageNumber?: number;
  pageSize?: number;
  createdAfter?: string;
  createdBefore?: string;
  minAmount?: number;
  maxAmount?: number;
}

export interface CreateOfflinePaymentInput {
  reference: string;
  amount: number;
  method: PaymentMethod;
}

export interface CreateOnlinePaymentInput {
  amount: number;
  reference: string;
  payerId: string;
  payerName: string;
  credential: string;
}

export interface CheckoutSession {
  code: string;
  qrCode: string;
}

export interface UpdatePaymentStatusInput {
  id: string;
  status: PaymentStatus;
}

export type PagedPayments = PagedResult<Payment>;
