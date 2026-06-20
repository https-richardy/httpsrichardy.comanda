import type { IntegrationDTO } from '../dto';
import type { Integration } from '../../domain/entities';

export const IntegrationsMapper = {
  toDomain(dto: IntegrationDTO): Integration {
    return {
      id: dto.id,
      name: dto.name,
      enabled: dto.enabled
    };
  }
};

export default IntegrationsMapper;
