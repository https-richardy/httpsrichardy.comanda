import { OrdersAPI } from '../../data/api';
import { OrderMapper } from '../../data/mapper';
import type {
  Order,
  OrderFilters,
  CreateOrderInput,
  UpdateOrderInput,
  PagedOrders
} from '../entities';

export class OrderService {
  static async list(filters?: OrderFilters): Promise<PagedOrders> {
    return OrdersAPI.list(filters);
  }

  static async create(input: CreateOrderInput): Promise<Order> {
    const dto = OrderMapper.toCreateDTO(input);
    return OrdersAPI.create(dto);
  }

  static async update(input: UpdateOrderInput): Promise<Order> {
    const dto = OrderMapper.toUpdateDTO(input);
    return OrdersAPI.update(input.id, dto);
  }
}
