import { AxiosError } from 'axios';

export function handleOrderError(
  error: Error | unknown,
  action: string = 'operação de pedido'
): never {
  let message = `Ocorreu um erro inesperado durante ${action}.`;

  if (error instanceof AxiosError) {
    const status = error.response?.status;
    const apiMessage = error.response?.data?.message;

    switch (status) {
      case 400:
        message = apiMessage || 'Dados do pedido inválidos.';
        break;
      case 401:
        message = 'Sem autorização para realizar esta operação.';
        break;
      case 403:
        message = 'Acesso negado.';
        break;
      case 404:
        message = apiMessage || 'Pedido não encontrado.';
        break;
      case 409:
        message = apiMessage || 'Conflito ao processar o pedido.';
        break;
      case 422:
        message = apiMessage || 'Status inválido para esta transição.';
        break;
      default:
        message = apiMessage || message;
    }
  } else if (error instanceof Error) {
    message = error.message;
  }

  throw new Error(message);
}
