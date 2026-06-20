import { EstablishmentsAPI, ProductsAPI } from '../../data/api';
import { EstablishmentMapper, ProductMapper } from '../../data/mapper';
import type {
  Establishment,
  EstablishmentFilters,
  CreateEstablishmentInput,
  UpdateEstablishmentInput,
  PagedEstablishments,
  Product,
  ProductFilters,
  CreateProductInput,
  UpdateProductInput,
  DeleteProductInput,
  UploadProductImageInput,
  PagedProducts
} from '../entities';

export class EstablishmentService {
  static async list(filters?: EstablishmentFilters): Promise<PagedEstablishments> {
    return EstablishmentsAPI.list(filters);
  }

  static async create(input: CreateEstablishmentInput): Promise<Establishment> {
    const dto = EstablishmentMapper.toCreateDTO(input);
    return EstablishmentsAPI.create(dto);
  }

  static async update(input: UpdateEstablishmentInput): Promise<Establishment> {
    const dto = EstablishmentMapper.toUpdateDTO(input);
    return EstablishmentsAPI.update(input.id, dto);
  }

  static async delete(id: string): Promise<void> {
    return EstablishmentsAPI.delete(id);
  }
}

export class ProductService {
  static async list(
    establishmentId: string,
    filters?: ProductFilters
  ): Promise<PagedProducts> {
    return ProductsAPI.list(establishmentId, filters);
  }

  static async create(input: CreateProductInput): Promise<Product> {
    const dto = ProductMapper.toCreateDTO(input);
    return ProductsAPI.create(input.establishmentId, dto);
  }

  static async update(input: UpdateProductInput): Promise<Product> {
    const dto = ProductMapper.toUpdateDTO(input);
    return ProductsAPI.update(input.establishmentId, input.productId, dto);
  }

  static async delete(input: DeleteProductInput): Promise<void> {
    return ProductsAPI.delete(input.establishmentId, input.productId);
  }

  static async uploadImage(input: UploadProductImageInput): Promise<void> {
    return ProductsAPI.uploadImage(
      input.establishmentId,
      input.productId,
      input.file
    );
  }
}
