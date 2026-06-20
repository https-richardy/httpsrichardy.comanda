import { useGetKitchenItems } from '../hooks/use-queries';

export function KitchenPage() {
  const { data, isLoading } = useGetKitchenItems();

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Cozinha</h1>

      {isLoading && <p>Carregando itens da cozinha...</p>}

      {!isLoading && (
        <div>
          {data && data.length > 0 ? (
            <ul>
              {data.map((item) => (
                <li key={item.id} className="py-2 border-b">
                  <div className="font-medium">{item.name}</div>
                  <div className="text-sm text-muted-foreground">{item.status}</div>
                </li>
              ))}
            </ul>
          ) : (
            <div className="text-muted-foreground">Nenhum item encontrado.</div>
          )}
        </div>
      )}
    </main>
  );
}

export default KitchenPage;
