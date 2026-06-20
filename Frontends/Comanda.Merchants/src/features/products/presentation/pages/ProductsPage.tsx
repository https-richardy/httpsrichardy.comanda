import { useGetProducts } from '../hooks/use-queries';

export function ProductsPage() {
  const { data, isLoading } = useGetProducts();

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Produtos</h1>

      {isLoading && <p>Carregando produtos...</p>}

      {!isLoading && (
        <div>
          {data && data.length > 0 ? (
            <ul>
              {data.map((p) => (
                <li key={p.id} className="py-2 border-b">
                  <div className="font-medium">{p.name}</div>
                  <div className="text-sm text-muted-foreground">R$ {p.price} — Estoque: {p.stock}</div>
                </li>
              ))}
            </ul>
          ) : (
            <div className="text-muted-foreground">Nenhum produto encontrado.</div>
          )}
        </div>
      )}
    </main>
  );
}

export default ProductsPage;
