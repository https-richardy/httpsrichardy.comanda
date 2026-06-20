import type { PagedResult } from '@/shared/types/paginated-response';

// Establishment entities

export interface EstablishmentOwner {
  id: string;
  name: string;
}

export interface EstablishmentBranding {
  primaryColor: string;
  secondaryColor: string;
  logo: string;
}

export interface Establishment {
  id: string;
  title: string;
  description: string;
  owner: EstablishmentOwner;
  branding: EstablishmentBranding;
}

export interface EstablishmentFilters {
  id?: string;
  ownerId?: string;
  title?: string;
  pageNumber?: number;
  pageSize?: number;
  createdAfter?: string;
  createdBefore?: string;
}

export interface CreateEstablishmentInput {
  title: string;
  description: string;
  ownerId: string;
  ownerName: string;
}

export interface UpdateEstablishmentInput {
  id: string;
  title: string;
  description: string;
  branding: EstablishmentBranding;
}

export type PagedEstablishments = PagedResult<Establishment>;

// Product entities

export interface Product {
  id: string;
  title: string;
  description: string;
  image: string;
  price: number;
}

export interface ProductFilters {
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

export interface CreateProductInput {
  establishmentId: string;
  title: string;
  description: string;
  price: number;
}

export interface UpdateProductInput {
  establishmentId: string;
  productId: string;
  title: string;
  description: string;
  price: number;
}

export interface DeleteProductInput {
  establishmentId: string;
  productId: string;
}

export interface UploadProductImageInput {
  establishmentId: string;
  productId: string;
  file: File;
}

export type PagedProducts = PagedResult<Product>;
