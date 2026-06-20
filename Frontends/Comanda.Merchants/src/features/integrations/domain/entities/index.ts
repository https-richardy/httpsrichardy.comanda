export type IntegrationProvider = 'PaymentGateway' | 'Whatsapp';

export interface Credential {
  id: string;
  provider: IntegrationProvider;
  secretKey: string;
}

export interface CreateCredentialInput {
  establishmentId: string;
  secretKey: string;
  provider: IntegrationProvider;
}

export interface UpdateCredentialInput {
  establishmentId: string;
  secretKey: string;
  provider: IntegrationProvider;
}
