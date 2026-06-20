import { Badge } from '@/shared/components/ui/badge';
import { Card, CardContent, CardHeader, CardTitle } from '@/shared/components/ui/card';

import { useKitchenOrders } from '../hooks/use-queries';
import { useUpdateOrder } from '@/features/orders/presentation/hooks/use-mutations';
import type { Order } from '../../domain/entities';

function KitchenCard({ order }: { order: Order }) {
  const { mutate: updateOrder, isPending } = useUpdateOrder();

  const advance = () => {
    if (order.status === 'Confirmed') {
      updateOrder({ id: order.id, status: 'InPreparation', priority: order.priority });
    } else if (order.status === 'InPreparation') {
      updateOrder({ id: order.id, status: 'Ready', priority: order.priority });
    }
  };

  return (
    <Card className="cursor-pointer hover:border-primary transition-colors" onClick={advance}>
      <CardHeader className="pb-2">
        <CardTitle className="text-base font-mono">{order.code}</CardTitle>
      </CardHeader>
      <CardContent className="space-y-1">
        {order.items.map((item, i) => (
          <div key={i} className="text-sm">
            <span className="font-medium">{item.quantity}x</span> {item.title}
          </div>
        ))}
        <div className="text-xs text-muted-foreground pt-1">
          {isPending ? 'Atualizando...' : 'Clique para avançar'}
        </div>
      </CardContent>
    </Card>
  );
}

export function KitchenPage() {
  const { confirmed, inPreparation, isLoading } = useKitchenOrders();

  return (
    <main className="p-6">
      <h1 className="text-2xl font-bold mb-6">Cozinha</h1>

      {isLoading && (
        <p className="text-muted-foreground">Carregando pedidos...</p>
      )}

      {!isLoading && (
        <div className="grid grid-cols-2 gap-6">
          <section>
            <div className="flex items-center gap-2 mb-3">
              <h2 className="text-lg font-semibold">Confirmados</h2>
              <Badge variant="outline">{confirmed.length}</Badge>
            </div>
            <div className="space-y-3">
              {confirmed.length === 0 ? (
                <p className="text-sm text-muted-foreground">Nenhum pedido confirmado.</p>
              ) : (
                confirmed.map((o) => <KitchenCard key={o.id} order={o} />)
              )}
            </div>
          </section>

          <section>
            <div className="flex items-center gap-2 mb-3">
              <h2 className="text-lg font-semibold">Em Preparo</h2>
              <Badge variant="secondary">{inPreparation.length}</Badge>
            </div>
            <div className="space-y-3">
              {inPreparation.length === 0 ? (
                <p className="text-sm text-muted-foreground">Nenhum pedido em preparo.</p>
              ) : (
                inPreparation.map((o) => <KitchenCard key={o.id} order={o} />)
              )}
            </div>
          </section>
        </div>
      )}
    </main>
  );
}
