import type { CredentialDTO, CreateCredentialDTO, UpdateCredentialDTO } from '../dto';
import type {
  Credential,
  CreateCredentialInput,
  UpdateCredentialInput
} from '../../domain/entities';

export class CredentialMapper {
  static toModel(dto: CredentialDTO): Credential {
    return {
      id: dto.identifier,
      provider: dto.provider,
      secretKey: dto.secretKey
    };
  }

  static toModelList(dtos: CredentialDTO[]): Credential[] {
    return dtos.map(CredentialMapper.toModel);
  }

  static toCreateDTO(input: CreateCredentialInput): CreateCredentialDTO {
    return {
      secretKey: input.secretKey,
      provider: input.provider
    };
  }

  static toUpdateDTO(input: UpdateCredentialInput): UpdateCredentialDTO {
    return {
      secretKey: input.secretKey,
      provider: input.provider
    };
  }
}
