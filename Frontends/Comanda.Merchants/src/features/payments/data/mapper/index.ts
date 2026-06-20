import type { PaymentDTO, CreateOfflinePaymentDTO, CreateOnlinePaymentDTO, UpdatePaymentStatusDTO } from '../dto';
import type {
  Payment,
  CreateOfflinePaymentInput,
  CreateOnlinePaymentInput,
  UpdatePaymentStatusInput
} from '../../domain/entities';

export class PaymentMapper {
  static toModel(dto: PaymentDTO): Payment {
    return {
      id: dto.identifier,
      reference: dto.reference,
      amount: dto.amount,
      payer: { id: dto.payer.identifier, name: dto.payer.username },
      status: dto.status,
      method: dto.method
    };
  }

  static toModelList(dtos: PaymentDTO[]): Payment[] {
    return dtos.map(PaymentMapper.toModel);
  }

  static toCreateOfflineDTO(input: CreateOfflinePaymentInput): CreateOfflinePaymentDTO {
    return {
      reference: input.reference,
      amount: input.amount,
      method: input.method
    };
  }

  static toCreateOnlineDTO(input: CreateOnlinePaymentInput): CreateOnlinePaymentDTO {
    return {
      amount: input.amount,
      reference: input.reference,
      payer: { identifier: input.payerId, username: input.payerName }
    };
  }

  static toUpdateStatusDTO(input: UpdatePaymentStatusInput): UpdatePaymentStatusDTO {
    return { status: input.status };
  }
}
