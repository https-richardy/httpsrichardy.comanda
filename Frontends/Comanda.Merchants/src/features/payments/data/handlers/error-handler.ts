import { AxiosError } from 'axios';

export function handlePaymentError(
  error: Error | unknown,
  action: string = 'operação de pagamento'
): never {
  let message = `Ocorreu um erro inesperado durante ${action}.`;

  if (error instanceof AxiosError) {
    const status = error.response?.status;
    const apiMessage = error.response?.data?.message;

    switch (status) {
      case 400:
        message = apiMessage || 'Dados de pagamento inválidos.';
        break;
      case 401:
        message = 'Sem autorização. Verifique as credenciais de pagamento.';
        break;
      case 402:
        message = apiMessage || 'Pagamento recusado.';
        break;
      case 404:
        message = apiMessage || 'Pagamento não encontrado.';
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
