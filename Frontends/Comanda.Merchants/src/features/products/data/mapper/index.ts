import type { ProductDTO } from '../dto';
import type { Product } from '../../domain/entities';

export const ProductsMapper = {
  toDomain(dto: ProductDTO): Product {
    return {
      id: dto.id,
      name: dto.name,
      price: dto.price,
      stock: dto.stock
    };
  }
};

export default ProductsMapper;
