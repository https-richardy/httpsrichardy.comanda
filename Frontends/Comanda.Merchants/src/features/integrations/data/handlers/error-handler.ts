import { AxiosError } from 'axios';

export function handleIntegrationError(
  error: Error | unknown,
  action: string = 'operação de integração'
): never {
  let message = `Ocorreu um erro inesperado durante ${action}.`;

  if (error instanceof AxiosError) {
    const status = error.response?.status;
    const apiMessage = error.response?.data?.message;

    switch (status) {
      case 400:
        message = apiMessage || 'Dados de integração inválidos.';
        break;
      case 401:
        message = 'Sem autorização para gerenciar integrações.';
        break;
      case 404:
        message = apiMessage || 'Integração não encontrada.';
        break;
      case 409:
        message = apiMessage || 'Já existe uma integração com este provedor.';
        break;
      default:
        message = apiMessage || message;
    }
  } else if (error instanceof Error) {
    message = error.message;
  }

  throw new Error(message);
}
