import { PaymentsAPI } from '../../data/api';
import { PaymentMapper } from '../../data/mapper';
import type {
  Payment,
  PaymentFilters,
  CreateOfflinePaymentInput,
  CreateOnlinePaymentInput,
  CheckoutSession,
  UpdatePaymentStatusInput,
  PagedPayments
} from '../entities';

export class PaymentService {
  static async list(filters?: PaymentFilters): Promise<PagedPayments> {
    return PaymentsAPI.list(filters);
  }

  static async createOffline(input: CreateOfflinePaymentInput): Promise<Payment> {
    const dto = PaymentMapper.toCreateOfflineDTO(input);
    return PaymentsAPI.createOffline(dto);
  }

  static async createOnline(input: CreateOnlinePaymentInput): Promise<CheckoutSession> {
    const dto = PaymentMapper.toCreateOnlineDTO(input);
    return PaymentsAPI.createOnline(dto, input.credential);
  }

  static async updateStatus(input: UpdatePaymentStatusInput): Promise<Payment> {
    const dto = PaymentMapper.toUpdateStatusDTO(input);
    return PaymentsAPI.updateStatus(input.id, dto);
  }
}
