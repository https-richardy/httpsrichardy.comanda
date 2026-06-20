import type { PaginationMetadata } from '@/shared/types/paginated-response';
import { httpClient } from '@/infra/api/gateway-api';

import type {
  PaymentDTO,
  PaymentFiltersDTO,
  CreateOfflinePaymentDTO,
  CreateOnlinePaymentDTO,
  CheckoutSessionDTO,
  UpdatePaymentStatusDTO
} from '../dto';
import { handlePaymentError } from '../handlers/error-handler';
import { PaymentMapper } from '../mapper';
import type { Payment, CheckoutSession, PagedPayments } from '../../domain/entities';

export class PaymentsAPI {
  static async list(filters?: PaymentFiltersDTO): Promise<PagedPayments> {
    try {
      const params: Record<string, string | number | undefined> = {
        id: filters?.id,
        payerId: filters?.payerId,
        externalId: filters?.externalId,
        referenceId: filters?.referenceId,
        status: filters?.status,
        method: filters?.method,
        createdAfter: filters?.createdAfter,
        createdBefore: filters?.createdBefore,
        minAmount: filters?.minAmount,
        maxAmount: filters?.maxAmount,
        'pagination.pageNumber': filters?.pageNumber ?? 1,
        'pagination.pageSize': filters?.pageSize ?? 20
      };

      Object.keys(params).forEach(
        (k) => params[k] === undefined && delete params[k]
      );

      const { data, headers } = await httpClient.get<PaymentDTO[]>(
        '/api/v1/payments',
        { params }
      );

      const paginationHeader = headers['x-pagination'];
      const pagination: PaginationMetadata = paginationHeader
        ? JSON.parse(paginationHeader)
        : {
            total: data.length,
            totalPages: 1,
            pageNumber: 1,
            pageSize: data.length,
            hasNext: false,
            hasPrevious: false
          };

      return { items: PaymentMapper.toModelList(data), pagination };
    } catch (error) {
      handlePaymentError(error, 'buscar pagamentos');
    }
  }

  static async createOffline(input: CreateOfflinePaymentDTO): Promise<Payment> {
    try {
      const { data } = await httpClient.post<PaymentDTO>(
        '/api/v1/payments/offline',
        input
      );
      return PaymentMapper.toModel(data);
    } catch (error) {
      handlePaymentError(error, 'registrar pagamento offline');
    }
  }

  static async createOnline(
    input: CreateOnlinePaymentDTO,
    credential: string
  ): Promise<CheckoutSession> {
    try {
      const { data } = await httpClient.post<CheckoutSessionDTO>(
        '/api/v1/payments/online',
        input,
        { headers: { 'X-Credential': credential } }
      );
      return { code: data.code, qrCode: data.qrCode };
    } catch (error) {
      handlePaymentError(error, 'criar cobrança online');
    }
  }

  static async updateStatus(
    id: string,
    input: UpdatePaymentStatusDTO
  ): Promise<Payment> {
    try {
      const { data } = await httpClient.put<PaymentDTO>(
        `/api/v1/payments/${id}/status`,
        input
      );
      return PaymentMapper.toModel(data);
    } catch (error) {
      handlePaymentError(error, 'atualizar status do pagamento');
    }
  }
}
