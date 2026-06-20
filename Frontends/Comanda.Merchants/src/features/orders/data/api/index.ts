import type { PaginationMetadata } from '@/shared/types/paginated-response';
import { httpClient } from '@/infra/api/gateway-api';

import type { OrderDTO, OrderFiltersDTO, CreateOrderDTO, UpdateOrderDTO } from '../dto';
import { handleOrderError } from '../handlers/error-handler';
import { OrderMapper } from '../mapper';
import type { Order, PagedOrders } from '../../domain/entities';

export class OrdersAPI {
  static async list(filters?: OrderFiltersDTO): Promise<PagedOrders> {
    try {
      const params: Record<string, string | number | undefined> = {
        id: filters?.id,
        code: filters?.code,
        status: filters?.status,
        priority: filters?.priority,
        fulfillment: filters?.fulfillment,
        merchantId: filters?.merchantId,
        consumerId: filters?.consumerId,
        createdAfter: filters?.createdAfter,
        createdBefore: filters?.createdBefore,
        'pagination.pageNumber': filters?.pageNumber ?? 1,
        'pagination.pageSize': filters?.pageSize ?? 20
      };

      Object.keys(params).forEach(
        (k) => params[k] === undefined && delete params[k]
      );

      const { data, headers } = await httpClient.get<OrderDTO[]>(
        '/api/v1/orders',
        { params }
      );

      const paginationHeader = headers['x-pagination'];
      const pagination: PaginationMetadata = paginationHeader
        ? JSON.parse(paginationHeader)
        : {
            total: data.length,
            totalPages: 1,
            pageNumber: 1,
            pageSize: data.length,
            hasNext: false,
            hasPrevious: false
          };

      return { items: OrderMapper.toModelList(data), pagination };
    } catch (error) {
      handleOrderError(error, 'buscar pedidos');
    }
  }

  static async create(input: CreateOrderDTO): Promise<Order> {
    try {
      const { data } = await httpClient.post<OrderDTO>('/api/v1/orders', input);
      return OrderMapper.toModel(data);
    } catch (error) {
      handleOrderError(error, 'criar pedido');
    }
  }

  static async update(id: string, input: UpdateOrderDTO): Promise<Order> {
    try {
      const { data } = await httpClient.put<OrderDTO>(
        `/api/v1/orders/${id}`,
        input
      );
      return OrderMapper.toModel(data);
    } catch (error) {
      handleOrderError(error, 'atualizar pedido');
    }
  }
}
