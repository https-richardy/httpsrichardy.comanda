import { AxiosError } from 'axios';

export function handleAuthError(
  error: Error | unknown,
  action: string = 'autenticação'
): never {
  let message = `Ocorreu um erro inesperado durante ${action}.`;

  if (error instanceof AxiosError) {
    const status = error.response?.status;
    const apiMessage = error.response?.data?.message;

    switch (status) {
      case 400:
        message = apiMessage || 'Dados de login inválidos.';
        break;
      case 401:
        message =
          apiMessage || 'Credenciais inválidas. Verifique seu usuário e senha.';
        break;
      case 403:
        message = 'Acesso negado. Você não tem permissão para acessar.';
        break;
      case 404:
        message = 'Serviço de autenticação não encontrado.';
        break;
      case 429:
        message = 'Muitas tentativas de login. Tente novamente mais tarde.';
        break;
      case 500:
        message =
          'Erro interno no servidor durante autenticação. Tente novamente.';
        break;
      case 503:
        message = 'Serviço de autenticação temporariamente indisponível.';
        break;
      default:
        message = apiMessage || message;
    }
  } else if (error instanceof Error) {
    message = error.message;
  }

  throw new Error(message);
}
