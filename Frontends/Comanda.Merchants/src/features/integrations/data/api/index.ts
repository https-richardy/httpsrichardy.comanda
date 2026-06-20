import { httpClient } from '@/infra/api/gateway-api';

import type {
  CredentialDTO,
  CreateCredentialDTO,
  UpdateCredentialDTO
} from '../dto';
import { handleIntegrationError } from '../handlers/error-handler';
import { CredentialMapper } from '../mapper';
import type { Credential } from '../../domain/entities';

export class CredentialsAPI {
  static async list(establishmentId: string): Promise<Credential[]> {
    try {
      const { data } = await httpClient.get<CredentialDTO[]>(
        `/api/v1/stores/${establishmentId}/integrations/credentials`
      );
      return CredentialMapper.toModelList(data);
    } catch (error) {
      handleIntegrationError(error, 'buscar credenciais');
    }
  }

  static async create(
    establishmentId: string,
    input: CreateCredentialDTO
  ): Promise<Credential> {
    try {
      const { data } = await httpClient.post<CredentialDTO>(
        `/api/v1/stores/${establishmentId}/integrations/credentials`,
        input
      );
      return CredentialMapper.toModel(data);
    } catch (error) {
      handleIntegrationError(error, 'criar credencial');
    }
  }

  static async update(
    establishmentId: string,
    input: UpdateCredentialDTO
  ): Promise<Credential> {
    try {
      const { data } = await httpClient.put<CredentialDTO>(
        `/api/v1/stores/${establishmentId}/integrations/credentials`,
        input
      );
      return CredentialMapper.toModel(data);
    } catch (error) {
      handleIntegrationError(error, 'atualizar credencial');
    }
  }
}
