import { httpClient } from '@/infra/api/gateway-api';

import type { KitchenItemDTO } from '../dto';

export class KitchenAPI {
  static async list(): Promise<KitchenItemDTO[]> {
    const { data } = await httpClient.get<KitchenItemDTO[]>('/kitchen');
    return data;
  }
}
