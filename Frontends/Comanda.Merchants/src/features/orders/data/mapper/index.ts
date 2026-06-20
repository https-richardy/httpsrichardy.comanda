import type { OrderDTO } from '../dto';
import type { Order } from '../../domain/entities';

export const OrdersMapper = {
  toDomain(dto: OrderDTO): Order {
    return {
      id: dto.id,
      status: dto.status,
      total: dto.total,
      createdAt: new Date(dto.createdAt)
    };
  }
};

export default OrdersMapper;
