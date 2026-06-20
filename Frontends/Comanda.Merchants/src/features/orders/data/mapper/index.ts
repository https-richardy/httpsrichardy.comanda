import type { OrderDTO, CreateOrderDTO, UpdateOrderDTO } from '../dto';
import type { Order, CreateOrderInput, UpdateOrderInput } from '../../domain/entities';

export class OrderMapper {
  static toModel(dto: OrderDTO): Order {
    return {
      id: dto.identifier,
      code: dto.code,
      address: dto.address,
      status: dto.status,
      priority: dto.priority,
      fulfillment: dto.fulfillment,
      items: dto.items.map((item) => ({
        title: item.title,
        description: item.description,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        totalPrice: item.totalPrice
      })),
      merchantId: dto.metadata.merchantId,
      consumerId: dto.metadata.consumerId
    };
  }

  static toModelList(dtos: OrderDTO[]): Order[] {
    return dtos.map(OrderMapper.toModel);
  }

  static toCreateDTO(input: CreateOrderInput): CreateOrderDTO {
    return {
      fulfillment: input.fulfillment,
      priority: input.priority,
      address: input.address,
      metadata: { merchantId: input.merchantId, consumerId: input.consumerId },
      items: input.items
    };
  }

  static toUpdateDTO(input: UpdateOrderInput): UpdateOrderDTO {
    return {
      status: input.status,
      priority: input.priority
    };
  }
}
