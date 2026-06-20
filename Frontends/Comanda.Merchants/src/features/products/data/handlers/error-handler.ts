import { AxiosError } from 'axios';

export function handleProductError(
  error: Error | unknown,
  action: string = 'operação de produto'
): never {
  let message = `Ocorreu um erro inesperado durante ${action}.`;

  if (error instanceof AxiosError) {
    const status = error.response?.status;
    const apiMessage = error.response?.data?.message;

    switch (status) {
      case 400:
        message = apiMessage || 'Dados inválidos.';
        break;
      case 401:
        message = 'Sem autorização para realizar esta operação.';
        break;
      case 403:
        message = 'Acesso negado.';
        break;
      case 404:
        message = apiMessage || 'Recurso não encontrado.';
        break;
      case 413:
        message = 'Imagem muito grande. Tente um arquivo menor.';
        break;
      default:
        message = apiMessage || message;
    }
  } else if (error instanceof Error) {
    message = error.message;
  }

  throw new Error(message);
}
