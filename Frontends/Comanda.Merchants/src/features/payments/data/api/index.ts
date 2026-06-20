import { httpClient } from '@/infra/api/gateway-api';

import type { PaymentDTO } from '../dto';

export class PaymentsAPI {
  static async list(): Promise<PaymentDTO[]> {
    const { data } = await httpClient.get<PaymentDTO[]>('/payments');
    return data;
  }
}
