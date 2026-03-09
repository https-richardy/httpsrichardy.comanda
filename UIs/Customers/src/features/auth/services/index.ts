
import type {
  AuthSession,
  LoginCredentials,
} from '../types';


export const authService = {
  login: async (credentials: LoginCredentials): Promise<AuthSession> => {
    const now = new Date()
    console.log(now)
    const expiresAt = new Date(now.getFullYear(), now.getMonth(), now.getDay(), now.getHours() + 2, now.getMinutes(), now.getSeconds())
    console.log(expiresAt)

    return {
      token: "TOKENT.EMPLATE.EXAMPLE",
      status: "authenticated",
      expiresAt: expiresAt.getTime(),
      user: {
        name: "User Template Example",
        email: credentials.username,
        id: "123456",
      }
    };
  },

  logout: async () => {
    return Promise.resolve();
  }
};
