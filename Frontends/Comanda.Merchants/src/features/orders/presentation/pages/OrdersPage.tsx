import { useState } from 'react';
import { Badge } from '@/shared/components/ui/badge';
import { Button } from '@/shared/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/shared/components/ui/table';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/shared/components/ui/select';

import { useOrders } from '../hooks/use-queries';
import { useUpdateOrder } from '../hooks/use-mutations';
import type { OrderStatus, Order } from '../../domain/entities';

const statusLabel: Record<OrderStatus, string> = {
  Pending: 'Pendente',
  Confirmed: 'Confirmado',
  InPreparation: 'Em Preparo',
  Ready: 'Pronto',
  Finalized: 'Finalizado',
  Cancelled: 'Cancelado',
  Failed: 'Falhou',
  Refunded: 'Reembolsado',
  Returned: 'Devolvido'
};

const statusVariant: Record<
  OrderStatus,
  'default' | 'secondary' | 'destructive' | 'outline'
> = {
  Pending: 'outline',
  Confirmed: 'default',
  InPreparation: 'secondary',
  Ready: 'default',
  Finalized: 'secondary',
  Cancelled: 'destructive',
  Failed: 'destructive',
  Refunded: 'outline',
  Returned: 'outline'
};

const fulfillmentLabel = {
  Unspecified: '-',
  Pickup: 'Retirada',
  Delivery: 'Entrega'
};

function OrderRow({ order }: { order: Order }) {
  const { mutate: updateOrder, isPending } = useUpdateOrder();

  const handleStatusChange = (status: OrderStatus) => {
    updateOrder({ id: order.id, status, priority: order.priority });
  };

  const total = order.items.reduce((sum, item) => sum + item.totalPrice, 0);

  return (
    <TableRow>
      <TableCell className="font-mono text-xs">{order.code}</TableCell>
      <TableCell>
        <Badge variant={statusVariant[order.status]}>
          {statusLabel[order.status]}
        </Badge>
      </TableCell>
      <TableCell>{fulfillmentLabel[order.fulfillment]}</TableCell>
      <TableCell>{order.items.length} item(s)</TableCell>
      <TableCell className="text-right">
        R$ {total.toFixed(2)}
      </TableCell>
      <TableCell>
        <Select
          value={order.status}
          onValueChange={(v: string) => handleStatusChange(v as OrderStatus)}
          disabled={isPending}
        >
          <SelectTrigger className="w-40">
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            {(Object.keys(statusLabel) as OrderStatus[]).map((s) => (
              <SelectItem key={s} value={s}>
                {statusLabel[s]}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </TableCell>
    </TableRow>
  );
}

export function OrdersPage() {
  const [page, setPage] = useState(1);
  const { data, isLoading } = useOrders({ pageNumber: page, pageSize: 20 });

  return (
    <main className="p-6 space-y-4">
      <h1 className="text-2xl font-bold">Pedidos</h1>

      {isLoading && (
        <p className="text-muted-foreground">Carregando pedidos...</p>
      )}

      {!isLoading && (
        <>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Código</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Tipo</TableHead>
                <TableHead>Itens</TableHead>
                <TableHead className="text-right">Total</TableHead>
                <TableHead>Alterar status</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items.length ? (
                data.items.map((order) => (
                  <OrderRow key={order.id} order={order} />
                ))
              ) : (
                <TableRow>
                  <TableCell
                    colSpan={6}
                    className="text-center text-muted-foreground py-8"
                  >
                    Nenhum pedido encontrado.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>

          {data && data.pagination.totalPages > 1 && (
            <div className="flex justify-end gap-2">
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage((p) => Math.max(1, p - 1))}
                disabled={!data.pagination.hasPrevious}
              >
                Anterior
              </Button>
              <span className="text-sm self-center text-muted-foreground">
                {data.pagination.pageNumber} / {data.pagination.totalPages}
              </span>
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage((p) => p + 1)}
                disabled={!data.pagination.hasNext}
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
