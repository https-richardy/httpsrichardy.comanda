import { httpClient } from '@/infra/api/gateway-api';

import type { OrderDTO } from '../dto';

export class OrdersAPI {
  static async list(): Promise<OrderDTO[]> {
    const { data } = await httpClient.get<OrderDTO[]>('/orders');
    return data;
  }
}
