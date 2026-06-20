import { useState } from 'react';
import { Button } from '@/shared/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/shared/components/ui/table';

import { useCurrentEstablishment, useProducts } from '../hooks/use-queries';
import { useDeleteProduct } from '../hooks/use-mutations';
import type { Product } from '../../domain/entities';

function ProductRow({
  product,
  establishmentId
}: {
  product: Product;
  establishmentId: string;
}) {
  const { mutate: deleteProduct, isPending } = useDeleteProduct();

  return (
    <TableRow>
      <TableCell>
        {product.image ? (
          <img
            src={product.image}
            alt={product.title}
            className="w-10 h-10 object-cover rounded"
          />
        ) : (
          <div className="w-10 h-10 bg-muted rounded" />
        )}
      </TableCell>
      <TableCell className="font-medium">{product.title}</TableCell>
      <TableCell className="text-sm text-muted-foreground max-w-xs truncate">
        {product.description}
      </TableCell>
      <TableCell className="text-right font-mono">
        R$ {product.price.toFixed(2)}
      </TableCell>
      <TableCell>
        <Button
          variant="destructive"
          size="sm"
          disabled={isPending}
          onClick={() =>
            deleteProduct({
              establishmentId,
              productId: product.id
            })
          }
        >
          {isPending ? 'Removendo...' : 'Remover'}
        </Button>
      </TableCell>
    </TableRow>
  );
}

export function ProductsPage() {
  const [page, setPage] = useState(1);
  const { data: establishment, isLoading: loadingEstablishment } =
    useCurrentEstablishment();
  const { data: products, isLoading: loadingProducts } = useProducts(
    establishment?.id,
    { pageNumber: page, pageSize: 20 }
  );

  const isLoading = loadingEstablishment || loadingProducts;

  return (
    <main className="p-6 space-y-4">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold">Produtos</h1>
          {establishment && (
            <p className="text-sm text-muted-foreground">{establishment.title}</p>
          )}
        </div>
      </div>

      {isLoading && (
        <p className="text-muted-foreground">Carregando produtos...</p>
      )}

      {!loadingEstablishment && !establishment && (
        <div className="text-center py-12 text-muted-foreground">
          <p>Nenhum estabelecimento encontrado para sua conta.</p>
        </div>
      )}

      {!isLoading && establishment && (
        <>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="w-14">Imagem</TableHead>
                <TableHead>Nome</TableHead>
                <TableHead>Descrição</TableHead>
                <TableHead className="text-right">Preço</TableHead>
                <TableHead className="w-28">Ações</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {products?.items.length ? (
                products.items.map((product) => (
                  <ProductRow
                    key={product.id}
                    product={product}
                    establishmentId={establishment.id}
                  />
                ))
              ) : (
                <TableRow>
                  <TableCell
                    colSpan={5}
                    className="text-center text-muted-foreground py-8"
                  >
                    Nenhum produto cadastrado.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>

          {products && products.pagination.totalPages > 1 && (
            <div className="flex justify-end gap-2">
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage((p) => Math.max(1, p - 1))}
                disabled={!products.pagination.hasPrevious}
              >
                Anterior
              </Button>
              <span className="text-sm self-center text-muted-foreground">
                {products.pagination.pageNumber} / {products.pagination.totalPages}
              </span>
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage((p) => p + 1)}
                disabled={!products.pagination.hasNext}
              >
                Próxima
              </Button>
            </div>
          )}
        </>
      )}
    </main>
  );
}
