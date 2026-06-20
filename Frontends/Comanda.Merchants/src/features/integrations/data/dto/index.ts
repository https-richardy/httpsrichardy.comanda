export type IntegrationProvider = 'PaymentGateway' | 'Whatsapp';

export interface CredentialDTO {
  identifier: string;
  provider: IntegrationProvider;
  secretKey: string;
}

export interface CreateCredentialDTO {
  secretKey: string;
  provider: IntegrationProvider;
}

export interface UpdateCredentialDTO {
  secretKey: string;
  provider: IntegrationProvider;
}
