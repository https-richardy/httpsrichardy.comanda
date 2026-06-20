export type SessionStatus =
  | 'loading'
  | 'authenticated'
  | 'unauthenticated'
  | 'expired'
  | 'idle';

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface User {
  id: string;
  email: string;
  name: string;
  firstName: string;
  lastName: string;
  roles: string[];
}

export interface AuthSession {
  token: string | null;
  expiresAt: number;
  user: User;
  status: SessionStatus;
}
