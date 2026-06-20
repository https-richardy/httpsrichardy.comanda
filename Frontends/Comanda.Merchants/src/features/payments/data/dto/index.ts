export type PaymentStatus = 'Pending' | 'Paid' | 'Refunded' | 'Failed';

export type PaymentMethod = 'Unspecified' | 'Pix' | 'Card' | 'Cash';

export interface PaymentUserDTO {
  identifier: string;
  username: string;
}

export interface PaymentDTO {
  identifier: string;
  reference: string;
  amount: number;
  payer: PaymentUserDTO;
  status: PaymentStatus;
  method: PaymentMethod;
}

export interface PaymentFiltersDTO {
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

export interface CreateOfflinePaymentDTO {
  reference: string;
  amount: number;
  method: PaymentMethod;
}

export interface CreateOnlinePaymentDTO {
  amount: number;
  reference: string;
  payer: PaymentUserDTO;
}

export interface CheckoutSessionDTO {
  code: string;
  qrCode: string;
}

export interface UpdatePaymentStatusDTO {
  status: PaymentStatus;
}
