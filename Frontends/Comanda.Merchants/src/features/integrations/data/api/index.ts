import { httpClient } from '@/infra/api/gateway-api';

import type { IntegrationDTO } from '../dto';

export class IntegrationsAPI {
  static async list(): Promise<IntegrationDTO[]> {
    const { data } = await httpClient.get<IntegrationDTO[]>('/integrations');
    return data;
  }
}
