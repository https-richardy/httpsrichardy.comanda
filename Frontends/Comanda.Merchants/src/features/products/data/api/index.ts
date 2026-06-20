import type { PaginationMetadata } from '@/shared/types/paginated-response';
import { httpClient } from '@/infra/api/gateway-api';

import type {
  EstablishmentDTO,
  EstablishmentFiltersDTO,
  CreateEstablishmentDTO,
  UpdateEstablishmentDTO,
  ProductDTO,
  ProductFiltersDTO,
  CreateProductDTO,
  UpdateProductDTO
} from '../dto';
import { handleProductError } from '../handlers/error-handler';
import { EstablishmentMapper, ProductMapper } from '../mapper';
import type {
  Establishment,
  Product,
  PagedEstablishments,
  PagedProducts
} from '../../domain/entities';

function parsePagination(headers: Record<string, string>, fallbackLength: number): PaginationMetadata {
  const header = headers['x-pagination'];
  return header
    ? JSON.parse(header)
    : {
        total: fallbackLength,
        totalPages: 1,
        pageNumber: 1,
        pageSize: fallbackLength,
        hasNext: false,
        hasPrevious: false
      };
}

function cleanParams(
  obj: Record<string, string | number | undefined>
): Record<string, string | number> {
  return Object.fromEntries(
    Object.entries(obj).filter(([, v]) => v !== undefined)
  ) as Record<string, string | number>;
}

export class EstablishmentsAPI {
  static async list(filters?: EstablishmentFiltersDTO): Promise<PagedEstablishments> {
    try {
      const params = cleanParams({
        id: filters?.id,
        ownerId: filters?.ownerId,
        title: filters?.title,
        createdAfter: filters?.createdAfter,
        createdBefore: filters?.createdBefore,
        'pagination.pageNumber': filters?.pageNumber ?? 1,
        'pagination.pageSize': filters?.pageSize ?? 20
      });

      const { data, headers } = await httpClient.get<EstablishmentDTO[]>(
        '/api/v1/stores',
        { params }
      );

      return {
        items: EstablishmentMapper.toModelList(data),
        pagination: parsePagination(headers as Record<string, string>, data.length)
      };
    } catch (error) {
      handleProductError(error, 'buscar estabelecimentos');
    }
  }

  static async create(input: CreateEstablishmentDTO): Promise<Establishment> {
    try {
      const { data } = await httpClient.post<EstablishmentDTO>(
        '/api/v1/stores',
        input
      );
      return EstablishmentMapper.toModel(data);
    } catch (error) {
      handleProductError(error, 'criar estabelecimento');
    }
  }

  static async update(
    id: string,
    input: UpdateEstablishmentDTO
  ): Promise<Establishment> {
    try {
      const { data } = await httpClient.put<EstablishmentDTO>(
        `/api/v1/stores/${id}`,
        input
      );
      return EstablishmentMapper.toModel(data);
    } catch (error) {
      handleProductError(error, 'atualizar estabelecimento');
    }
  }

  static async delete(id: string): Promise<void> {
    try {
      await httpClient.delete(`/api/v1/stores/${id}`);
    } catch (error) {
      handleProductError(error, 'deletar estabelecimento');
    }
  }
}

export class ProductsAPI {
  static async list(
    establishmentId: string,
    filters?: ProductFiltersDTO
  ): Promise<PagedProducts> {
    try {
      const params = cleanParams({
        id: filters?.id,
        title: filters?.title,
        minPrice: filters?.minPrice,
        maxPrice: filters?.maxPrice,
        createdAfter: filters?.createdAfter,
        createdBefore: filters?.createdBefore,
        'pagination.pageNumber': filters?.pageNumber ?? 1,
        'pagination.pageSize': filters?.pageSize ?? 20
      });

      const { data, headers } = await httpClient.get<ProductDTO[]>(
        `/api/v1/stores/${establishmentId}/products`,
        { params }
      );

      return {
        items: ProductMapper.toModelList(data),
        pagination: parsePagination(headers as Record<string, string>, data.length)
      };
    } catch (error) {
      handleProductError(error, 'buscar produtos');
    }
  }

  static async create(
    establishmentId: string,
    input: CreateProductDTO
  ): Promise<Product> {
    try {
      const { data } = await httpClient.post<ProductDTO>(
        `/api/v1/stores/${establishmentId}/products`,
        input
      );
      return ProductMapper.toModel(data);
    } catch (error) {
      handleProductError(error, 'criar produto');
    }
  }

  static async update(
    establishmentId: string,
    productId: string,
    input: UpdateProductDTO
  ): Promise<Product> {
    try {
      const { data } = await httpClient.put<ProductDTO>(
        `/api/v1/stores/${establishmentId}/products/${productId}`,
        input
      );
      return ProductMapper.toModel(data);
    } catch (error) {
      handleProductError(error, 'atualizar produto');
    }
  }

  static async delete(
    establishmentId: string,
    productId: string
  ): Promise<void> {
    try {
      await httpClient.delete(
        `/api/v1/stores/${establishmentId}/products/${productId}`
      );
    } catch (error) {
      handleProductError(error, 'deletar produto');
    }
  }

  static async uploadImage(
    establishmentId: string,
    productId: string,
    file: File
  ): Promise<void> {
    try {
      const form = new FormData();
      form.append('file', file);
      await httpClient.put(
        `/api/v1/stores/${establishmentId}/products/${productId}/image`,
        form,
        { headers: { 'Content-Type': 'multipart/form-data' } }
      );
    } catch (error) {
      handleProductError(error, 'enviar imagem do produto');
    }
  }
}
