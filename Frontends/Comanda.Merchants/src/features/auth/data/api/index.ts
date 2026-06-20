import { federationClient } from '@/infra/api/federation-api';

import type { AuthSession, LoginCredentials } from '../../domain/entities';
import type { LoginResponseDTO } from '../dto';
import { handleAuthError } from '../handlers/error-handler';
import { AuthMapper } from '../mapper';

export class AuthAPI {
  static async login(credentials: LoginCredentials): Promise<AuthSession> {
    try {
      const payload = AuthMapper.toLoginDTO(credentials);

      const { data } = await federationClient.post<LoginResponseDTO>(
        '/api/v1/users/authenticate',
        payload
      );

      return AuthMapper.toDomain(data);
    } catch (error) {
      handleAuthError(error, 'login');
    }
  }
}
