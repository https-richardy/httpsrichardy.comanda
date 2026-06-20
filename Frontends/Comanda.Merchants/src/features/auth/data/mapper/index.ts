import { jwtDecode } from 'jwt-decode';

import type { AuthSession, User } from '../../domain/entities';
import type { JwtPayloadDTO, LoginRequestDTO, LoginResponseDTO } from '../dto';
import type { LoginCredentials } from '../../domain/entities';

export class AuthMapper {
  static toDomain(dto: LoginResponseDTO): AuthSession {
    const decoded = jwtDecode<JwtPayloadDTO>(dto.accessToken);

    const nameParts = (decoded.name ?? '').split(' ');

    const user: User = {
      id: decoded.sub,
      email: decoded.email ?? '',
      name: decoded.name ?? decoded.email ?? '',
      firstName: nameParts[0] ?? '',
      lastName: nameParts.slice(1).join(' '),
      roles:
        typeof decoded.groups === 'string'
          ? [decoded.groups]
          : (decoded.groups ?? [])
    };

    return {
      token: dto.accessToken,
      user,
      status: 'authenticated',
      expiresAt: decoded.exp * 1000
    };
  }

  static toLoginDTO(credentials: LoginCredentials): LoginRequestDTO {
    return {
      username: credentials.username,
      password: credentials.password
    };
  }
}
