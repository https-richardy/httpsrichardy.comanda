import type {
  EstablishmentDTO,
  ProductDTO,
  CreateEstablishmentDTO,
  UpdateEstablishmentDTO,
  CreateProductDTO,
  UpdateProductDTO
} from '../dto';
import type {
  Establishment,
  Product,
  CreateEstablishmentInput,
  UpdateEstablishmentInput,
  CreateProductInput,
  UpdateProductInput
} from '../../domain/entities';

export class EstablishmentMapper {
  static toModel(dto: EstablishmentDTO): Establishment {
    return {
      id: dto.identifier,
      title: dto.title,
      description: dto.description,
      owner: { id: dto.owner.identifier, name: dto.owner.username },
      branding: {
        primaryColor: dto.branding?.primaryColor ?? '',
        secondaryColor: dto.branding?.secondaryColor ?? '',
        logo: dto.branding?.logo ?? ''
      }
    };
  }

  static toModelList(dtos: EstablishmentDTO[]): Establishment[] {
    return dtos.map(EstablishmentMapper.toModel);
  }

  static toCreateDTO(input: CreateEstablishmentInput): CreateEstablishmentDTO {
    return {
      title: input.title,
      description: input.description,
      owner: { identifier: input.ownerId, username: input.ownerName }
    };
  }

  static toUpdateDTO(input: UpdateEstablishmentInput): UpdateEstablishmentDTO {
    return {
      title: input.title,
      description: input.description,
      branding: input.branding
    };
  }
}

export class ProductMapper {
  static toModel(dto: ProductDTO): Product {
    return {
      id: dto.identifier,
      title: dto.title,
      description: dto.description,
      image: dto.image,
      price: dto.price
    };
  }

  static toModelList(dtos: ProductDTO[]): Product[] {
    return dtos.map(ProductMapper.toModel);
  }

  static toCreateDTO(input: CreateProductInput): CreateProductDTO {
    return {
      title: input.title,
      description: input.description,
      price: input.price
    };
  }

  static toUpdateDTO(input: UpdateProductInput): UpdateProductDTO {
    return {
      title: input.title,
      description: input.description,
      price: input.price
    };
  }
}
