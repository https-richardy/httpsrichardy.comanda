import { AuthAPI } from '../../data/api';
import type { AuthSession, LoginCredentials } from '../entities';

export class authService {
  static async login(credentials: LoginCredentials): Promise<AuthSession> {
    return AuthAPI.login(credentials);
  }

  static async logout() {
    return Promise.resolve();
  }

  static isSessionValid(expiresAt: number | null): boolean {
    if (!expiresAt) return false;

    return Date.now() < expiresAt;
  }
}
