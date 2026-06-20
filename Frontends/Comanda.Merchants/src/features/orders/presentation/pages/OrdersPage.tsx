import { useGetOrders } from '../hooks/use-queries';

export function OrdersPage() {
  const { data, isLoading } = useGetOrders();

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Pedidos</h1>

      {isLoading && <p>Carregando pedidos...</p>}

      {!isLoading && (
        <div>
          {data && data.length > 0 ? (
            <ul>
              {data.map((order) => (
                <li key={order.id} className="py-2 border-b">
                  <div className="font-medium">Pedido: {order.id}</div>
                  <div className="text-sm text-muted-foreground">
                    Status: {order.status} — Total: R$ {order.total}
                  </div>
                </li>
              ))}
            </ul>
          ) : (
            <div className="text-muted-foreground">Nenhum pedido encontrado.</div>
          )}
        </div>
      )}
    </main>
  );
}

export default OrdersPage;
