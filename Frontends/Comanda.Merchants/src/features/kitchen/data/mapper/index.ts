import type { KitchenItemDTO } from '../dto';
import type { KitchenItem } from '../../domain/entities';

export const KitchenMapper = {
  toDomain(dto: KitchenItemDTO): KitchenItem {
    return {
      id: dto.id,
      name: dto.name,
      status: dto.status
    };
  }
};

export default KitchenMapper;
