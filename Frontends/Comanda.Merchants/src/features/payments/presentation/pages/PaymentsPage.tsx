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

import { usePayments } from '../hooks/use-queries';
import type { PaymentStatus, PaymentMethod } from '../../domain/entities';

const statusLabel: Record<PaymentStatus, string> = {
  Pending: 'Pendente',
  Paid: 'Pago',
  Refunded: 'Reembolsado',
  Failed: 'Falhou'
};

const statusVariant: Record<
  PaymentStatus,
  'default' | 'secondary' | 'destructive' | 'outline'
> = {
  Pending: 'outline',
  Paid: 'default',
  Refunded: 'secondary',
  Failed: 'destructive'
};

const methodLabel: Record<PaymentMethod, string> = {
  Unspecified: '-',
  Pix: 'Pix',
  Card: 'Cartão',
  Cash: 'Dinheiro'
};

export function PaymentsPage() {
  const [page, setPage] = useState(1);
  const { data, isLoading } = usePayments({ pageNumber: page, pageSize: 20 });

  return (
    <main className="p-6 space-y-4">
      <h1 className="text-2xl font-bold">Pagamentos</h1>

      {isLoading && (
        <p className="text-muted-foreground">Carregando pagamentos...</p>
      )}

      {!isLoading && (
        <>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Referência</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Método</TableHead>
                <TableHead>Pagador</TableHead>
                <TableHead className="text-right">Valor</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items.length ? (
                data.items.map((payment) => (
                  <TableRow key={payment.id}>
                    <TableCell className="font-mono text-xs">
                      {payment.reference}
                    </TableCell>
                    <TableCell>
                      <Badge variant={statusVariant[payment.status]}>
                        {statusLabel[payment.status]}
                      </Badge>
                    </TableCell>
                    <TableCell>{methodLabel[payment.method]}</TableCell>
                    <TableCell>{payment.payer.name}</TableCell>
                    <TableCell className="text-right">
                      R$ {payment.amount.toFixed(2)}
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell
                    colSpan={5}
                    className="text-center text-muted-foreground py-8"
                  >
                    Nenhum pagamento encontrado.
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
