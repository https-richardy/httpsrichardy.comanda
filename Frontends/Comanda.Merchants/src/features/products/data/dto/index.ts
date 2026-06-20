// Establishment DTOs

export interface EstablishmentOwnerDTO {
  identifier: string;
  username: string;
}

export interface EstablishmentBrandingDTO {
  primaryColor: string;
  secondaryColor: string;
  logo: string;
}

export interface EstablishmentDTO {
  identifier: string;
  title: string;
  description: string;
  owner: EstablishmentOwnerDTO;
  branding: EstablishmentBrandingDTO;
}

export interface EstablishmentFiltersDTO {
  id?: string;
  ownerId?: string;
  title?: string;
  pageNumber?: number;
  pageSize?: number;
  createdAfter?: string;
  createdBefore?: string;
}

export interface CreateEstablishmentDTO {
  title: string;
  description: string;
  owner: EstablishmentOwnerDTO;
}

export interface UpdateEstablishmentDTO {
  title: string;
  description: string;
  branding: EstablishmentBrandingDTO;
}

// Product DTOs

export interface ProductDTO {
  identifier: string;
  title: string;
  description: string;
  image: string;
  price: number;
}

export interface ProductFiltersDTO {
  id?: string;
  establishmentId?: string;
  title?: string;
  minPrice?: number;
  maxPrice?: number;
  pageNumber?: number;
  pageSize?: number;
  createdAfter?: string;
  createdBefore?: string;
}

export interface CreateProductDTO {
  title: string;
  description: string;
  price: number;
}

export interface UpdateProductDTO {
  title: string;
  description: string;
  price: number;
}
