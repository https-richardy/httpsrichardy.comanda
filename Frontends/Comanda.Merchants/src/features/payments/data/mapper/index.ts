import type { PaymentDTO } from '../dto';
import type { Payment } from '../../domain/entities';

export const PaymentsMapper = {
  toDomain(dto: PaymentDTO): Payment {
    return {
      id: dto.id,
      method: dto.method,
      amount: dto.amount,
      status: dto.status
    };
  }
};

export default PaymentsMapper;
