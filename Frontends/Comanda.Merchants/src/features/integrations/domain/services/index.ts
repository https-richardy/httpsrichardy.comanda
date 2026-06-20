import { CredentialsAPI } from '../../data/api';
import { CredentialMapper } from '../../data/mapper';
import type {
  Credential,
  CreateCredentialInput,
  UpdateCredentialInput
} from '../entities';

export class CredentialService {
  static async list(establishmentId: string): Promise<Credential[]> {
    return CredentialsAPI.list(establishmentId);
  }

  static async create(input: CreateCredentialInput): Promise<Credential> {
    const dto = CredentialMapper.toCreateDTO(input);
    return CredentialsAPI.create(input.establishmentId, dto);
  }

  static async update(input: UpdateCredentialInput): Promise<Credential> {
    const dto = CredentialMapper.toUpdateDTO(input);
    return CredentialsAPI.update(input.establishmentId, dto);
  }
}
