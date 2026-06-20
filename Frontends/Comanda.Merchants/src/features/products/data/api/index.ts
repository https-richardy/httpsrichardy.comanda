import { httpClient } from '@/infra/api/gateway-api';

import type { ProductDTO } from '../dto';

export class ProductsAPI {
  static async list(): Promise<ProductDTO[]> {
    const { data } = await httpClient.get<ProductDTO[]>('/products');
    return data;
  }
}
